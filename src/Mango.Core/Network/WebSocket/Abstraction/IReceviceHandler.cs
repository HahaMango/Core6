namespace Mango.Core.Network.WebSocket.Abstraction
{
    /// <summary>
    /// webSocket接受消息接口
    /// </summary>
    public interface IReceviceHandler
    {
        /// <summary>
        /// handler
        /// </summary>
        /// <param name="webSocket">webSocket客户端</param>
        /// <param name="result">接收到的消息描述</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task Handler(MangoWebSocket webSocket, MangoWebSocket.ReceiveResult result, CancellationToken cancellationToken);
    }
}
