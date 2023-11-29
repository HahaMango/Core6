using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.EntityFramework.DataStructure
{
    /// <summary>
    /// 分页结构
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageList<T>
    {
        /// <summary>
        /// 分页内容
        /// </summary>
        public IEnumerable<T> Data { get; set; }

        /// <summary>
        /// 页数
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPage { get; set; }

        /// <summary>
        /// 当前页项数量
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public PageList()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="count"></param>
        /// <param name="data"></param>
        public PageList(int page, int size, int count, IEnumerable<T> data)
        {
            Page = page;
            Size = size;
            Count = count;
            TotalPage = (count / size) + 1;
            Data = data;
        }
    }
}
