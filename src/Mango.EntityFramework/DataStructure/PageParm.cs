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
        /// 页数 从1开始
        /// </summary>
        [Required(ErrorMessage = "页数不能为空")]
        public int Page { get; set; }

        /// <summary>
        /// 页大小
        /// </summary>
        public int Size { get; set; } = 20;

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set;}

        /// <summary>
        /// 时间字段
        /// </summary>
        public string? TimeField { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string? OrderByField { get; set; }

        /// <summary>
        /// 排序 0：顺序 1：倒序
        /// </summary>
        public int? Sort { get; set; }
    }
}
