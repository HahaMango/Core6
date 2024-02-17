using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Mango.EntityFramework.Abstractions.Repositories
{
    /// <summary>
    /// EF仓储基础接口
    /// </summary>
    public interface IEfRepository<TDbcontext,TEntity> : IBaseRepository<TEntity>
        where TEntity : class, IEntity , new()
        where TDbcontext : BaseDbContext
    {
        /// <summary>
        /// 工作单元
        /// </summary>
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// 跟踪
        /// </summary>
        IQueryable<TEntity> Table { get; }

        /// <summary>
        /// 非跟踪
        /// </summary>
        IQueryable<TEntity> TableNotTracking { get; }

        /// <summary>
        /// Db Set
        /// </summary>
        DbSet<TEntity> Entities { get; }

        /// <summary>
        /// 生成基础的where条件表达式树（用于动态构造查询条件）
        /// </summary>
        /// <returns></returns>
        Expression<Func<TEntity, bool>> GetQueryPredicate();
    }
}
