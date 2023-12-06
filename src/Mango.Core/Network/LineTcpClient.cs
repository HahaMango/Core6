using Mango.Core.IDGenerator;
using Mango.Core.IDGenerator.Abstractions;
using Mango.Core.Logger;
using Mango.Core.Network.Abstractions;
using Mango.Core.Serialization.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Pipelines;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Core.Network
{
    /// <summary>
    /// 以行（\n）为单位处理的TCP客户端
    /// </summary>
    public class LineTcpClient<T> : IMangoTcpClient
        where T : DataPackage,new()
    {
        /// <summary>
        /// 套接字
        /// </summary>
        private readonly Socket _socket;

        /// <summary>
        /// 代表套接字的网络流
        /// </summary>
        private NetworkStream _stream;

        /// <summary>
        /// 线程锁
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<LineTcpClient<T>> _logger;

        /// <summary>
        /// IP端口
        /// </summary>
        private readonly IPEndPoint _endpoint;

        /// <summary>
        /// 缓存接收到的响应
        /// </summary>
        private readonly BlockingCollection<T> _bcol;

        private readonly IGenerator<long> _keyGenerator;

        /// <summary>
        /// 创建行处理TCP客户端
        /// </summary>
        /// <param name="iP">IP</param>
        /// <param name="port">端口</param>
        public LineTcpClient(IPAddress iP, int port) : this(null, iP, port)
        {

        }

        /// <summary>
        /// 创建行处理TCP客户端（若logger参数为空则用ILoggerFactory初始化内部日志）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="iP">IP</param>
        /// <param name="port">端口</param>
        public LineTcpClient(ILogger<LineTcpClient<T>> logger, IPAddress iP,int port)
        {
            if (iP == null)
            {
                throw new ArgumentNullException(nameof(iP));
            }
            _endpoint = new IPEndPoint(iP, port);

            _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

            _bcol = new BlockingCollection<T>();

            _keyGenerator = SnowFlakeGenerator.Instance;

            if (logger == null)
            {
                _logger = LoggerHelper.Create<LineTcpClient<T>>();
                return;
            }
            _logger = logger;
        }

        /// <summary>
        /// 发送字节序列
        /// </summary>
        /// <param name="sendData"></param>
        /// <returns></returns>
        public Task SendAsync(byte[] sendData)
        {
            Connect();
            var stream = GetStream();
            var package = new T();
            package.Data = sendData;
            package.Id = _keyGenerator.GetKey();
            var s = Encoding.UTF8.GetBytes(package.ToJson() + "\n");
            lock (_lock)
            {
                stream.Write(s, 0, s.Length);
                stream.Flush();
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// 发送字节序列
        /// </summary>
        /// <param name="sendData"></param>
        /// <returns></returns>
        public async Task SendAsync(ReadOnlyMemory<byte> sendData)
        {
            await SendAsync(sendData);
        }

        /// <summary>
        /// 发送字节序列并等待响应
        /// </summary>
        /// <param name="sendData"></param>
        /// <returns></returns>
        public async Task<Memory<byte>> TakeResponseAsync(byte[] sendData)
        {
            Connect();
            var stream = GetStream();
            var id = _keyGenerator.GetKey();
            var package = new T();
            package.Data = sendData;
            package.Id = id;
            var s = Encoding.Unicode.GetBytes(package.ToJson() + "\n");
            lock (_lock)
            {
                stream.Write(s, 0, s.Length);
                stream.Flush();
            }
            var result = await Task.Run(() =>
            {
                return TakePackage(id);
            });

            return result.Data;
        }

       /// <summary>
       /// 发送字节序列并等待响应
       /// </summary>
       /// <param name="sendData"></param>
       /// <returns></returns>
        public async Task<Memory<byte>> TakeResponseAsync(ReadOnlyMemory<byte> sendData)
        {
            return await TakeResponseAsync(sendData);
        }

        private bool Connect()
        {
            if (!_socket.Connected)
            {
                _logger.LogInformation($"start connect to server {_endpoint.Address}:{_endpoint.Port}...");
                _socket.Connect(_endpoint);
                return true;
            }
            return false;
        }

        private Stream GetStream()
        {
            if(_socket == null)
            {
                throw new NullReferenceException(nameof(_socket));
            }
            if(_stream != null)
            {
                return _stream;
            }
            _stream = new NetworkStream(_socket);
            Task.Run(Revice);
            return _stream;
        }

        /// <summary>
        /// 接收线程
        /// </summary>
        /// <returns></returns>
        private async Task Revice()
        {
            try
            {
                var reader = PipeReader.Create(_stream);
                while (true)
                {
                    ReadResult result = await reader.ReadAsync();
                    ReadOnlySequence<byte> buffer = result.Buffer;

                    while (TryReadLine(ref buffer, out ReadOnlySequence<byte> line))
                    {
                        // Process the line.
                        var str = Encoding.Unicode.GetString(line.ToArray());
                        _bcol.Add(await str.ToObjectAsync<T>());
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
            }
            catch (System.Exception ex)
            {
                //服务端关闭触发异常
                _logger.LogError($"[{DateTime.Now}: 连接异常 {ex.Message}]");
                if(_stream != null && _socket.Connected)
                {
                    _ = Task.Run(Revice);
                }
            }
        }

        /// <summary>
        /// 读取行
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

        private T TakePackage(long id)
        {
            foreach(var p in _bcol.GetConsumingEnumerable())
            {
                if(p.Id == id)
                {
                    return p;
                }
                _bcol.Add(p);
            }
            return null;
        }
    }
}
