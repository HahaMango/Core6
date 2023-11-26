using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Core.Exception
{
    /// <summary>
    /// 分页参数异常
    /// </summary>
    public class InvalidPageParmException : ApplicationException
    {
        /// <summary>
        /// 分页参数异常
        /// </summary>
        public InvalidPageParmException():base(ExceptionMessageConstant.INVALID_PAGE_PARM)
        {
        }

        /// <summary>
        /// 分页参数异常
        /// </summary>
        /// <param name="message"></param>
        public InvalidPageParmException(string message) : base(message)
        {

        }
    }
}
