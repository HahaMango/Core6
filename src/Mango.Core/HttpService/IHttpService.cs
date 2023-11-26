using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Core.HttpService
{
    /// <summary>
    /// Http服务接口
    /// </summary>
    public interface IHttpService : IBaseHttp
    {
        /// <summary>
        /// 发送get请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> GetAsync(string url,string token = null);

        /// <summary>
        /// 发送post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> PostAsync(string url, string content,string token = null);
    }
}
