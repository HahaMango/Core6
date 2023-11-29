using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mango.EntityFramework.Abstractions.Repositories
{
    /// <summary>
    /// 聚合根仓储接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IAggregateRepository<T,TKey> where T:IAggregateRoot
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        Task AddAsync(T o);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        Task RemoveAsync(T o);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        Task UpdateAsync(T o);

        /// <summary>
        /// 查询聚合根
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetByIdAsync(TKey id);
    }
}
