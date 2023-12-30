using System.Net.WebSockets;
using System.Security.Policy;
using System.Text;
using Mango.Core.Network.WebSocket.Abstraction;
using Mango.Core.Serialization.Extension;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Mango.Core.Network.WebSocket
{
    public class MangoWebSocket : IDisposable
    {
        private readonly ILogger<MangoWebSocket> _logger;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        private ClientWebSocket _clientWebSocket;

        private Uri _uri;

        /// <summary>
        /// 接收消息线程
        /// </summary>
        private Task _receviceTask;

        private CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// 设置
        /// </summary>
        public MangoWebSocketOption Option { get; private set; }

        private bool disposedValue;

        /// <summary>
        /// 连接状态
        /// </summary>
        public WebSocketState? State => _clientWebSocket?.State;

        public MangoWebSocket(ILogger<MangoWebSocket> logger, IServiceScopeFactory serviceScopeFactory, MangoWebSocketOption option)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            Option = option;
        }

        public MangoWebSocket(ILogger<MangoWebSocket> logger, IServiceScopeFactory serviceScopeFactory, Uri uri, MangoWebSocketOption option) : this(logger, serviceScopeFactory, option)
        {
            _uri = uri;
        }

        /// <summary>
        /// 打开链接
        /// </summary>
        /// <returns></returns>
        public async Task OpenAsync()
        {
            await OpenAsync(_uri);
        }

        /// <summary>
        /// 打开链接
        /// </summary>
        public async Task OpenAsync(Uri uri)
        {
            _logger.LogInformation("开始连接websocket....");
            //初始化
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }
            _uri = uri;
            if (_clientWebSocket == null)
            {
                _clientWebSocket = new ClientWebSocket();
            }
            //判断当前连接状态
            if (_clientWebSocket.State == WebSocketState.Open)
            {
                _logger.LogInformation("当前连接为开启状态，无需建立连接....");
                return;
            }
            //如果连接被服务器主动关闭则先把连接关闭再重新链接
            if (_clientWebSocket.State == WebSocketState.CloseReceived)
            {
                _logger.LogInformation($"重新建立连接....");
                await CloseAsync();
                _clientWebSocket.Abort();
                _clientWebSocket = new ClientWebSocket();
            }
            //链接
            try
            {
                await _clientWebSocket.ConnectAsync(_uri, CancellationToken.None);
            }
            catch(Exception ex)
            {
                _logger.LogError($"建立连接异常：{ex.Message}");
                throw;
            }

            if(State == WebSocketState.Open)
            {
                if(Option.OnOpen != null)
                {
                    await Option.OnOpen(this, _logger);
                }
            }

            //开启后台线程接收返回的消息
            _cancellationTokenSource = new();
            var cancelToken = _cancellationTokenSource.Token;
            //如果接收消息handler列表有值，则生成一个长任务线程，执行从websocket中读取消息
            var handlerList = Option.ReceviceHandler;
            if (handlerList != null && handlerList.Count > 0)
            {
                _receviceTask = await Task.Factory.StartNew(async () =>
                {
                    _logger.LogInformation("开启ws后台消息线程...");
                    try
                    {
                        while (!cancelToken.IsCancellationRequested)
                        {
                            var receviceResult = await ReadAsync();
                            //在依赖注入容器中依次取出handlerTypeList列表中的handler执行
                            foreach (var handlerType in handlerList)
                            {
                                try
                                {
                                    //尝试从容器中取出handler
                                    using var scope = _serviceScopeFactory.CreateScope();
                                    var handler = (IReceviceHandler)scope.ServiceProvider.GetRequiredService(handlerType);
                                    //调用handler的方法
                                    await handler.Handler(this, receviceResult, cancelToken);
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError($"执行{handlerType.Name}异常：{ex.Message}");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"接收ws消息线程异常退出，链接状态：{State}，异常信息：{ex.Message}");
                        //判断链接状态，触发回调
                        if(State == WebSocketState.CloseReceived)
                        {
                            if (Option.OnCloseReceived != null)
                            {
                                await Option.OnCloseReceived(this, _logger);
                            }
                        }
                    }
                }
                , cancelToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
        }

        /// <summary>
        /// 关闭链接
        /// </summary>
        public async Task CloseAsync()
        {
            //判断接收消息线程是否在运行
            if (_receviceTask != null && _receviceTask.Status != TaskStatus.RanToCompletion && _receviceTask.Status != TaskStatus.Canceled && _receviceTask.Status != TaskStatus.Faulted)
            {
                //任务正在运行，执行取消动作
                _cancellationTokenSource.Cancel();
            }
            await _clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "NormalClosure", CancellationToken.None);

            _logger.LogInformation("CloseAsync()关闭链接...");

            //判断链接状态，触发回调
            if (State == WebSocketState.CloseSent)
            {
                if (Option.OnCloseSent != null)
                {
                    await Option.OnCloseSent(this, _logger);
                }
            }
            //判断链接状态，触发回调
            if (State == WebSocketState.Closed)
            {
                if (Option.OnClosed != null)
                {
                    await Option.OnClosed(this, _logger);
                }
            }
        }

        public async Task SendAsync(object body)
        {
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }
            SemaphoreSlim slimlock = new(1);
            try
            {
                await slimlock.WaitAsync();

                string bodyString;
                if (body is string)
                {
                    bodyString = body as string;
                }
                else
                {
                    bodyString = body.ToJson();
                }
                var sa = new ArraySegment<byte>(Encoding.UTF8.GetBytes(bodyString));
                await _clientWebSocket.SendAsync(sa, WebSocketMessageType.Text, true, CancellationToken.None);
                _logger.LogInformation($"成功发送消息：{bodyString}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"发送信息异常：{ex.Message}");
                throw;
            }
            finally
            {
                slimlock.Release();
            }
        }

        /// <summary>
        /// 读取消息
        /// </summary>
        /// <returns></returns>
        private async Task<ReceiveResult> ReadAsync()
        {
            SemaphoreSlim slimlock = new(1);
            try
            {
                await slimlock.WaitAsync();

                var contentBytes = new List<byte>();
                WebSocketReceiveResult result;
                var count = 0;
                do
                {
                    ArraySegment<byte> bytesReceived = new(new byte[1024]);
                    result = await _clientWebSocket.ReceiveAsync(bytesReceived, CancellationToken.None);
                    contentBytes.AddRange(bytesReceived);
                    count += result.Count;
                }
                while (!result.EndOfMessage);

                return new ReceiveResult
                {
                    MessageDesc = result,
                    Bytes = contentBytes.Take(count).ToArray()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"接收信息异常：{ex.Message}");
                throw;
            }
            finally
            {
                slimlock.Release();
            }
        }


        /// <summary>
        /// 接收消息结果
        /// </summary>
        public class ReceiveResult
        {
            /// <summary>
            /// 描述
            /// </summary>
            public WebSocketReceiveResult MessageDesc { get; set; }

            /// <summary>
            /// 返回内容字节
            /// </summary>
            public byte[] Bytes { get; set; }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                    _cancellationTokenSource.Cancel();
                    _clientWebSocket.Dispose();
                }

                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                _clientWebSocket = null;
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~FIWebSocket()
        // {
        //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// webSocket设置
    /// </summary>
    public class MangoWebSocketOption
    {
        private readonly List<Type> _receviceHandlerTypeList = new(4);

        public IReadOnlyCollection<Type> ReceviceHandler => _receviceHandlerTypeList;

        /// <summary>
        /// CloseReceived状态回调
        /// </summary>
        public Func<MangoWebSocket, ILogger<MangoWebSocket>, Task>? OnCloseReceived;

        /// <summary>
        /// CloseSent状态回调
        /// </summary>
        public Func<MangoWebSocket, ILogger<MangoWebSocket>, Task>? OnCloseSent;

        /// <summary>
        /// Closed状态回调
        /// </summary>
        public Func<MangoWebSocket, ILogger<MangoWebSocket>, Task>? OnClosed;

        /// <summary>
        /// Closed状态回调
        /// </summary>
        public Func<MangoWebSocket, ILogger<MangoWebSocket>, Task>? OnOpen;

        /// <summary>
        /// 添加接收消息handler
        /// </summary>
        /// <param name="handlerType">handler必须实现IReceviceHandler接口</param>
        public void AddHandler<T>() where T : IReceviceHandler
        {
            var handlerType = typeof(T);
            //判断类型是否实现IReceviceHandler接口
            if (!handlerType.GetInterfaces().Any(x => x == typeof(IReceviceHandler)))
            {
                throw new ArgumentException($"类型{handlerType}没有实现IReceviceHandler接口");
            }
            _receviceHandlerTypeList.Add(handlerType);
        }

        /// <summary>
        /// 删除接收消息handler
        /// </summary>
        /// <param name="handlerType">handler必须实现IReceviceHandler接口</param>
        public int RemoveHandler<T>() where T : IReceviceHandler
        {
            var handlerType = typeof(T);
            return _receviceHandlerTypeList.RemoveAll(x => x.FullName == handlerType.FullName);
        }
    }
}
