using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mango.EntityFramework.DataStructure
{
    /// <summary>
    /// 分页请求参数
    /// </summary>
    public class PageParm
    {
        /// <summary>
        /// 页数
        /// </summary>
        [Required(ErrorMessage = "页数不能为空")]
        public int Page { get; set; }

        /// <summary>
        /// 页大小
        /// </summary>
        public int Size { get; set; } = 20;
    }
}
