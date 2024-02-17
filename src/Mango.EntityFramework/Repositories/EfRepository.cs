using Mango.EntityFramework.Abstractions;
using Mango.EntityFramework.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mango.EntityFramework.Repositories
{
    /// <summary>
    /// 基础仓储实现（泛型）
    /// </summary>
    /// <typeparam name="TDbcontext"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public class EfRepository<TDbcontext, TEntity> : IEfRepository<TDbcontext, TEntity>
        where TDbcontext : BaseDbContext
        where TEntity : class, IEntity, new()
    {
        private readonly TDbcontext _context;
        private DbSet<TEntity> _entities;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public EfRepository(TDbcontext context)
        {
            _context = context;
        }

        /// <summary>
        /// 跟踪
        /// </summary>
        public IQueryable<TEntity> Table => Entities ;

        /// <summary>
        /// 非跟踪
        /// </summary>
        public IQueryable<TEntity> TableNotTracking => Entities.AsNoTracking();

        /// <summary>
        /// Db Set
        /// </summary>
        public DbSet<TEntity> Entities => _entities ?? (_entities = _context.Set<TEntity>());

        public IUnitOfWork UnitOfWork => _context;

        public void Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            _context.Remove(entity);
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            _context.RemoveRange(entities);
        }

        /// <summary>
        /// 基础查询条件表达式树
        /// </summary>
        /// <returns></returns>
        public Expression<Func<TEntity, bool>> GetQueryPredicate()
        {
            return item => true;
        }

        public void Insert(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            Entities.Add(entity);
        }

        public void Insert(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            Entities.AddRange(entities);
        }

        public async Task InsertAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            await Entities.AddAsync(entity);
        }

        public async Task InsertAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            await Entities.AddRangeAsync(entities);
        }

        public TEntity QueryById(object Id)
        {
            if (Id == null)
                throw new ArgumentNullException(nameof(Id));
            return Entities.Find(Id);
        }

        public async Task<TEntity> QueryByIdAsync(object Id)
        {
            if (Id == null)
                throw new ArgumentNullException(nameof(Id));
            return await Entities.FindAsync(Id);
        }

        public void Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            _context.Update(entity);
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            _context.UpdateRange(entities);
        }
    }
}
