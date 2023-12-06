using Mango.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Core.ApiResponse
{
    /// <summary>
    /// 返回json对象
    /// </summary>
    public class ApiResult
    {
        public ApiResult() { }

        public ApiResult(Code code, string? message)
        {
            Code = code;
            Message = message;
        }

        /// <summary>
        /// 返回状态码
        /// </summary>
        public Code Code { get; set; }

        /// <summary>
        /// 返回字符串说明
        /// </summary>
        public string? Message { get; set; }
    }

    /// <summary>
    /// 返回json对象，包含数据内容
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResult<T> : ApiResult
    {
        public ApiResult()
        {

        }

        public ApiResult(Code code, string? message, T? data) : base(code, message)
        {
            Data = data;
        }

        /// <summary>
        /// 返回的json数据内容
        /// </summary>
        public T? Data { get; set; }
    }
}
