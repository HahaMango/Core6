using Mango.EntityFramework.DataStructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mango.EntityFramework.Abstractions
{
    /// <summary>
    /// EF事务，上下文SaveChanges组件
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        IDbContextTransaction BeginTransaction();

        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        void Rollback();

        Task RollbackAsync(CancellationToken cancellationToken = default);

        void Commit();

        Task CommitAsync(CancellationToken cancellationToken = default);

        #region 原生SQL查询
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parm);

        Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? parm);

        Task<int> ExecuteAsync(string sql, object? parm);

        Task<PageList<T>> QueryPageAsync<T>(string sql, object? parm, PageParm pageParm);
        #endregion
    }
}
