using Mango.Core.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Core.Exceptions
{
    public class ServiceException : Exception
    {
        /// <summary>
        /// 异常码
        /// </summary>
        public Code Code { get; }

        /// <summary>
        /// 错误对象
        /// </summary>
        public object E { get; }

        public ServiceException(string message) : base(message)
        {
            Code = Code.Error;
        }

        public ServiceException(Code code, string message) : base(message)
        {
            Code = code;
        }

        public ServiceException(string message, object e) : base(message)
        {
            E = e;
        }

        /// <summary>
        /// 抛出服务异常
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="ServiceException"></exception>
        [DoesNotReturn]
        public static void Throw(string message)
        {
            throw new ServiceException(message);
        }

        /// <summary>
        /// 抛出服务异常
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <exception cref="ServiceException"></exception>
        [DoesNotReturn]
        public static void Throw(Code code, string message)
        {
            throw new ServiceException(code, message);
        }

        public static void Throw(string message, object e)
        {
            throw new ServiceException(message, e);
        }
    }
}
