using AspectCore.DynamicProxy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mango.EntityFramework.AOP
{
    /// <summary>
    /// 数据库事务AOP
    /// </summary>
    public class TransactionAttribute : AbstractInterceptorAttribute
    {
        private readonly Type _dbContextType;

        public TransactionAttribute(Type dbContextType)
        {
            if(dbContextType.BaseType != typeof(BaseDbContext))
            {
                throw new ArgumentException($"类型{_dbContextType}，不继承BaseDbContext！");
            }
            _dbContextType = dbContextType;
        }

        /// <summary>
        /// 拦截方法
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            var dbContext = (BaseDbContext)context.ServiceProvider.GetRequiredService(_dbContextType);
            var logger = context.ServiceProvider.GetRequiredService<ILogger<TransactionAttribute>>();

            //如果已经开启了事务，则直接执行被代理的业务代码
            if (dbContext.IsTransaction)
            {
                await next(context);
                return;
            }
            //开启事务
            try
            {
                //重试策略下执行手动事务
                var strategy = dbContext.Database.CreateExecutionStrategy();

                await strategy.ExecuteAsync(async () =>
                {
                    await dbContext.BeginTransactionAsync();
                    logger.LogInformation($"》》》》》》》开启{_dbContextType.Name}事务》》》》》》》》");

                    await next(context);

                    await dbContext.CommitAsync();
                    logger.LogInformation($"》》》》》》》提交{_dbContextType.Name}事务》》》》》》》》");
                });
            }
            catch
            {
                if (dbContext.IsTransaction)
                {
                    await dbContext.RollbackAsync();
                    logger.LogInformation($"》》》》》》》回滚{_dbContextType.Name}事务》》》》》》》》");
                }
                throw;
            }
            finally
            {
                //保底，避免catch也执行失败导致事务没有释放，从而导致死锁发生
                if (dbContext.IsTransaction)
                {
                    //如果到这里事务仍然没有被释放
                    await dbContext.RollbackAsync();
                    logger.LogInformation($"》》》》》》》回滚{_dbContextType.Name}事务》》》》》》》》");
                }
            }
        }
    }
}
