using Mango.Core.DataStructure;
using Mango.Core.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Mango.Core.Extension
{
    /// <summary>
    /// linq 扩展类
    /// </summary>
    public static class EnumerableExtension
    {
        /// <summary>
        /// 分页查询扩展方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static PageList<T> ToPageList<T>(this IEnumerable<T> enumerable,int page,int size)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }
            if (page < 1 || size < 0)
            {
                throw new InvalidPageParmException();
            }

            var count = enumerable.Count();
            var data = enumerable.Skip((page - 1) * size).Take(size).ToList();

            return new PageList<T>(page, size, count, data);
        }
    }
}
