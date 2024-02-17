using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Mango.EntityFramework.DataStructure;
using Mango.EntityFramework.Exception;
using System.Drawing;
using System.Globalization;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;

namespace Mango.EntityFramework.Extension
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
        /// 分页查询扩展异步方法（请勿在select中搭配实体自动映射方法，可能会出现查询翻译问题，请在select中进行手动映射）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="pageParm"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidPageParmException"></exception>
        public static async Task<PageList<T>> ToPageListAsync<T>(this IQueryable<T> queryable, PageParm pageParm)
        {
            if (queryable == null)
            {
                throw new ArgumentNullException(nameof(queryable));
            }
            if (pageParm != null)
            {
                //分页
                var page = pageParm.Page;
                var size = pageParm.Size;
                if (page < 1 || size < 0)
                {
                    throw new InvalidPageParmException();
                }

                #region 按时间字段筛选
                if (!string.IsNullOrWhiteSpace(pageParm.TimeField))
                {
                    #region 首字母大小写转换
                    var timeField = pageParm.TimeField;
                    char[] timeFieldChar = timeField.ToCharArray();
                    char firstLetter = timeFieldChar[0];
                    if ('a' <= firstLetter && firstLetter <= 'z')
                    {
                        firstLetter = (char)(firstLetter & ~0x20);
                        timeFieldChar[0] = firstLetter;
                        timeField = new string(timeFieldChar);
                    }
                    #endregion

                    Type type = typeof(T);
                    var property = type.GetProperty(timeField);
                    if (property == null)
                    {
                        throw new InvalidPageParmException($"排序字段{pageParm.TimeField}不存在");
                    }

                    if (pageParm.StartTime.HasValue)
                    {
                        var right = Expression.Constant(pageParm.StartTime.Value, property.PropertyType);
                        var item = Expression.Parameter(type);
                        var propertyAccess = Expression.MakeMemberAccess(item, property);
                        var gte = Expression.GreaterThanOrEqual(propertyAccess, right);
                        var whereExpression = Expression.Lambda(gte, item);
                        var resultExpression = Expression.Call(typeof(Queryable), "Where", new Type[] { type }, queryable.Expression, Expression.Quote(whereExpression));

                        queryable = queryable.Provider.CreateQuery<T>(resultExpression);
                    }

                    if (pageParm.EndTime.HasValue)
                    {
                        var right = Expression.Constant(pageParm.EndTime.Value, property.PropertyType);
                        var item = Expression.Parameter(type);
                        var propertyAccess = Expression.MakeMemberAccess(item, property);
                        var lte = Expression.LessThanOrEqual(propertyAccess, right);
                        var whereExpression = Expression.Lambda(lte, item);
                        var resultExpression = Expression.Call(typeof(Queryable), "Where", new Type[] { type }, queryable.Expression, Expression.Quote(whereExpression));

                        queryable = queryable.Provider.CreateQuery<T>(resultExpression);
                    }
                }
                #endregion

                #region 按排序字段排序
                if (!string.IsNullOrWhiteSpace(pageParm.OrderByField))
                {
                    //sortName 首字母大小写转换
                    #region 首字母大小写转换
                    var sortName = pageParm.OrderByField;
                    char[] sortNameChar = sortName.ToCharArray();
                    char firstLetter = sortNameChar[0];
                    if ('a' <= firstLetter && firstLetter <= 'z')
                    {
                        firstLetter = (char)(firstLetter & ~0x20);
                        sortNameChar[0] = firstLetter;
                        sortName = new string(sortNameChar);
                    }
                    #endregion

                    var sort = pageParm.Sort ?? 0;
                    var orderWay = sort == 0 ? "OrderBy" : "OrderByDescending";
                    Type type = typeof(T);
                    var property = type.GetProperty(sortName);
                    if (property == null)
                    {
                        throw new InvalidPageParmException($"排序字段{sortName}不存在");
                    }
                    var parmarer = Expression.Parameter(type);
                    var propertyAccess = Expression.MakeMemberAccess(parmarer, property);
                    var orderByExpression = Expression.Lambda(propertyAccess, parmarer);
                    var resultExpression = Expression.Call(typeof(Queryable), orderWay, new Type[] { type, property.PropertyType }, queryable.Expression, Expression.Quote(orderByExpression));

                    queryable = queryable.Provider.CreateQuery<T>(resultExpression);
                }
                #endregion

                var count = await queryable.CountAsync();
                var data = await queryable.Skip((page - 1) * size).Take(size).ToListAsync();

                return new PageList<T>(page, size, count, data);
            }
            else
            {
                //没有分页参数，直接列表输出
                var list = await queryable.ToListAsync();
                return new PageList<T>(0, 0, list.Count, list);
            }
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
