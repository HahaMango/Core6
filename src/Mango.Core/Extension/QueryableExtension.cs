using Mango.Core.DataStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Mango.Core.Exception;

namespace Mango.Core.Extension
{
    /// <summary>
    /// Queryable查询扩展类
    /// </summary>
    public static class QueryableExtension
    {
        /// <summary>
        /// 分页查询扩展异步方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static async Task<PageList<T>> ToPageListAsync<T>(this IQueryable<T> queryable, int page, int size)
        {
            if(queryable == null)
            {
                throw new ArgumentNullException(nameof(queryable));
            }
            if(page < 1 || size < 0)
            {
                throw new InvalidPageParmException();
            }

            var count = await queryable.CountAsync();
            var data = await queryable.Skip((page - 1) * size).Take(size).ToListAsync();

            return new PageList<T>(page, size, count, data);
        }

        /// <summary>
        /// 分页查询扩展方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static PageList<T> ToPageList<T>(this IQueryable<T> queryable, int page, int size)
        {
            if (queryable == null)
            {
                throw new ArgumentNullException(nameof(queryable));
            }
            if (page < 1 || size < 0)
            {
                throw new InvalidPageParmException();
            }

            var count = queryable.Count();
            var data = queryable.Skip((page - 1) * size).Take(size).ToList();

            return new PageList<T>(page, size, count, data);
        }
    }
}
