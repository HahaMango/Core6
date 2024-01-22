using Mango.EntityFramework.Abstractions;
using Mango.EntityFramework.BaseEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyModel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Mango.EntityFramework
{
    /// <summary>
    /// Mango基础EF上下文
    /// (由于DbContext本身不支持单实例多线程使用，所以这里对事务的操作不做额外的加锁。请确保该实例不会被用在多个并发线程中)
    /// </summary>
    public class BaseDbContext : DbContext, IUnitOfWork
    {
        private IDbContextTransaction? _dbContextTransaction;

        public bool IsTransaction => _dbContextTransaction != null;

        public BaseDbContext()
        {
        }

        public BaseDbContext(DbContextOptions options) : base(options)
        {

        }

        public IDbContextTransaction BeginTransaction()
        {
            if (_dbContextTransaction != null)
            {
                return _dbContextTransaction;
            }

            _dbContextTransaction = Database.BeginTransaction();
            return _dbContextTransaction;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_dbContextTransaction != null)
            {
                return _dbContextTransaction;
            }

            _dbContextTransaction = await Database.BeginTransactionAsync(cancellationToken);
            return _dbContextTransaction;
        }

        public void Commit()
        {
            if (_dbContextTransaction == null) throw new ArgumentNullException(nameof(_dbContextTransaction));
            try
            {
                SaveChanges();
                _dbContextTransaction.Commit();
            }
            catch
            {
                _dbContextTransaction.Rollback();
                throw;
            }
            finally
            {
                if (_dbContextTransaction != null)
                {
                    _dbContextTransaction.Dispose();
                    _dbContextTransaction = null;
                }
            }
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            if (_dbContextTransaction == null) throw new ArgumentNullException(nameof(_dbContextTransaction));
            try
            {
                await SaveChangesAsync(cancellationToken);
                await _dbContextTransaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await _dbContextTransaction.RollbackAsync(cancellationToken);
                throw;
            }
            finally
            {
                if (_dbContextTransaction != null)
                {
                    await _dbContextTransaction.DisposeAsync();
                    _dbContextTransaction = null;
                }
            }
        }

        public void Rollback()
        {
            try
            {
                _dbContextTransaction?.Rollback();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (_dbContextTransaction != null)
                {
                    _dbContextTransaction.Dispose();
                    _dbContextTransaction = null;
                }
            }
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await _dbContextTransaction.RollbackAsync(cancellationToken);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (_dbContextTransaction != null)
                {
                    await _dbContextTransaction.DisposeAsync();
                    _dbContextTransaction = null;
                }
            }
        }

        /// <summary>
        /// 初始化上下文
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assemblies = GetAssemblies();
            foreach(var assembly in assemblies)
            {
                var types = assembly.GetTypes()
                    .Where(type => !string.IsNullOrWhiteSpace(type.Namespace))
                    .Where(type => type.IsClass)
                    .Where(type => type.BaseType != null)
                    .Where(type => typeof(Entity).IsAssignableFrom(type));//&& !typeof(IDbTable).IsSubclassOf(type))直接或间接的实现

                foreach (var type in types)
                {
                    if (modelBuilder.Model.FindEntityType(type) != null || type.IsAbstract == true)
                        continue;
                    modelBuilder.Model.AddEntityType(type);
                }
            }
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// 加载Mango开头的引用程序集
        /// </summary>
        /// <returns></returns>
        private List<Assembly> GetAssemblies()
        {
            var result = new List<Assembly>();
            var assemblies = DependencyContext.Default.CompileLibraries
                .Where(item => item.Name.StartsWith("Mango"))
                .ToList();
            foreach(var assembly in assemblies)
            {
                result.Add(Assembly.Load(assembly.Name));
            }

            return result;
        }
    }
}
