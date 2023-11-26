using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Mango.Core.HttpService
{
    /// <summary>
    /// 表示一个HttpClient的相应
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HttpResponse<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// 标识http请求是否成功
        /// </summary>
        public bool IsSuccessStatusCode { get; set; }

        /// <summary>
        /// 请求返回的数据内容
        /// </summary>
        public T Data { get; set; }
    }
}
