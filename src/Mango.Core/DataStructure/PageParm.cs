using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Core.DataStructure
{
    /// <summary>
    /// 分页请求参数
    /// </summary>
    public class PageParm
    {
        /// <summary>
        /// 页数
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 页大小
        /// </summary>
        public int Size { get; set; }
    }
}
