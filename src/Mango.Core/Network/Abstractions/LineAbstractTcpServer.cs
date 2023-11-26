using Mango.Core.Logger;
using Mango.Core.Network.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Mango.Core.Network.Abstractions
{
    /// <summary>
    /// 以行（\n）为单位处理的TCP服务端
    /// 
    /// 该类为抽象类，需要实现类实现具体的数据处理Handle方法
    /// </summary>
    public abstract class LineAbstractTcpServer : IMangoTcpServer
    {
        /// <summary>
        /// 线程锁
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// 日志
        /// </summary>
        protected readonly ILogger<LineAbstractTcpServer> _logger;

        /// <summary>
        /// 套接字
        /// </summary>
        private Socket _socket;

        /// <summary>
        /// 创建行处理TCP服务端
        /// </summary>
        public LineAbstractTcpServer() : this(null)
        {

        }

        /// <summary>
        /// 创建行处理TCP服务端（若logger参数为空则用ILoggerFactory初始化内部日志）
        /// </summary>
        /// <param name="logger">配合依赖注入的参数</param>
        public LineAbstractTcpServer(ILogger<LineAbstractTcpServer> logger = null)
        {
            if(logger == null)
            {
                _logger = LoggerHelper.Create<LineAbstractTcpServer>();
                return;
            }
            _logger = logger;
        }

        /// <summary>
        /// 在指定端口开始监听
        /// </summary>
        /// <param name="address"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public async Task Start(IPAddress address, int port)
        {
            _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(new IPEndPoint(address, port));

            _logger.LogInformation($"start TCP listen to {address}:{port}");

            _socket.Listen(120);

            while (true)
            {
                var socket = await _socket.AcceptAsync();
                _ = ProcessLinesAsync(socket);
            }
        }

        /// <summary>
        /// 数据处理程序
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public abstract ValueTask<Memory<byte>> Handle(ReadOnlyMemory<byte> input);

        #region 辅助函数

        /// <summary>
        /// 处理单行消息并将相应发送到流中
        /// </summary>
        /// <param name="data"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        private async Task Process(ReadOnlyMemory<byte> data, NetworkStream stream)
        {
            _logger.LogInformation($"[{DateTime.Now}]: start process ...");
            var response = await Handle(data);
            byte[] result = new byte[response.Length + 1];
            response.CopyTo(result);
            result[result.Length - 1] = 10;
            //加锁确保一条完整消息的发送
            lock (_lock)
            {
                stream.Write(result, 0, result.Length);
                stream.Flush();
            }
        }

        /// <summary>
        /// 接收并处理单个TCP连接
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        private async Task ProcessLinesAsync(Socket socket)
        {
            _logger.LogInformation($"[{socket.RemoteEndPoint}]: connected");

            // Create a PipeReader over the network stream
            var stream = new NetworkStream(socket);
            var reader = PipeReader.Create(stream);
            var writer = PipeWriter.Create(stream);

            while (true)
            {
                ReadResult result = await reader.ReadAsync();
                ReadOnlySequence<byte> buffer = result.Buffer;

                while (TryReadLine(ref buffer, out ReadOnlySequence<byte> line))
                {
                    // Process the line.
                    _logger.LogInformation($"[{DateTime.Now}]: take line...");
                    ReadOnlyMemory<byte> memory = line.ToArray();
                    _ = Task.Run(() =>
                      {
                          Process(memory, stream);
                      });
                }

                // Tell the PipeReader how much of the buffer has been consumed.
                reader.AdvanceTo(buffer.Start, buffer.End);

                // Stop reading if there's no more data coming.
                if (result.IsCompleted)
                {
                    break;
                }
            }

            // Mark the PipeReader as complete.
            await reader.CompleteAsync();

            _logger.LogInformation($"[{socket.RemoteEndPoint}]: disconnected");
        }

        /// <summary>
        /// 提取单行数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        private bool TryReadLine(ref ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> line)
        {
            // Look for a EOL in the buffer.
            SequencePosition? position = buffer.PositionOf((byte)'\n');

            if (position == null)
            {
                line = default;
                return false;
            }

            // Skip the line + the \n.
            line = buffer.Slice(0, position.Value);
            buffer = buffer.Slice(buffer.GetPosition(1, position.Value));
            return true;
        }

        #endregion
    }
}
