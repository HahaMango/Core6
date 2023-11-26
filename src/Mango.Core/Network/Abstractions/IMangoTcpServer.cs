using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Core.Network.Abstractions
{
    /// <summary>
    /// TCP服务端接口
    /// </summary>
    public interface IMangoTcpServer
    {
        /// <summary>
        /// 开始在指定IP和端口监听
        /// </summary>
        /// <param name="address"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        Task Start(IPAddress address, int port);
    }
}
