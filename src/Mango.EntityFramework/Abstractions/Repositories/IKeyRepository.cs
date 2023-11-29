using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mango.EntityFramework.Abstractions.Repositories
{
    /// <summary>
    /// 基础仓储接口，可由EF或dapper实现
    /// </summary>
    public interface IKeyRepository<TEntity,TKey>
        where TEntity : class,IEntity,new()
    {
        #region select

        TEntity QueryById(TKey Id);

        Task<TEntity> QueryByIdAsync(TKey Id);

        #endregion

        #region insert

        void Insert(TEntity entity);

        void Insert(IEnumerable<TEntity> entities);

        Task InsertAsync(TEntity entity);

        Task InsertAsync(IEnumerable<TEntity> entities);

        #endregion

        #region update

        void Update(TEntity entity);

        void Update(IEnumerable<TEntity> entities);

        #endregion

        #region delete

        void Delete(TEntity entity);

        void Delete(IEnumerable<TEntity> entities);

        #endregion
    }
}
