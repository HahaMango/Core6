using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
