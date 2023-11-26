using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Core.Network.Abstractions
{
    /// <summary>
    /// TCP客户端接口
    /// </summary>
    public interface IMangoTcpClient
    {
        /// <summary>
        /// 发送数据并等待响应
        /// </summary>
        /// <param name="sendData"></param>
        /// <returns></returns>
        Task<Memory<byte>> TakeResponseAsync(byte[] sendData);

        /// <summary>
        /// 发送数据并等待响应
        /// </summary>
        /// <param name="sendData"></param>
        /// <returns></returns>
        Task<Memory<byte>> TakeResponseAsync(ReadOnlyMemory<byte> sendData);

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="sendData"></param>
        /// <returns></returns>
        Task SendAsync(byte[] sendData);

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="sendData"></param>
        /// <returns></returns>
        Task SendAsync(ReadOnlyMemory<byte> sendData);
    }
}
