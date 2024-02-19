using Mango.EntityFramework.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Mango.EntityFramework.Extension
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// 添加Mango DB上下文
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connnectionString"></param>
        /// <returns></returns>
        public static IServiceCollection AddMangoDbContext<TDbContext,TEFContextWork>(this IServiceCollection services,string connnectionString) 
            where TDbContext : BaseDbContext
            where TEFContextWork : class, IUnitOfWork
        {
            services.AddDbContext<TDbContext>(config =>
            {
                config.UseMySql(ServerVersion.AutoDetect(connnectionString));
            });
            services.AddScoped<IUnitOfWork, TEFContextWork>();
            return services;
        }

        /// <summary>
        /// 添加Mango DB上下文
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connnectionString"></param>
        /// <returns></returns>
        public static IServiceCollection AddMangoDbContext<TDbContext>(this IServiceCollection services, string connnectionString, string? migrationsAssemblyName = null)
            where TDbContext : BaseDbContext
        {
            services.AddDbContext<TDbContext>(config =>
            {
                config.UseMySql(connnectionString, ServerVersion.AutoDetect(connnectionString), op =>
                {
                    op.EnableRetryOnFailure();
                    if (!string.IsNullOrWhiteSpace(migrationsAssemblyName))
                    {
                        op.MigrationsAssembly(migrationsAssemblyName);
                    }
                });
            });
            services.AddScoped<IUnitOfWork>(sp =>
            {
                return sp.GetRequiredService<TDbContext>();
            });
            return services;
        }
    }
}
