using Hangfire;
using Hangfire.Console;
using Hangfire.HttpJob;
using Hangfire.HttpJob.Agent.MysqlConsole;
using Hangfire.MySql;
using Mango.Core.Helper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System.Reflection;
using System.Transactions;
using MySqlStorageOptions = Hangfire.MySql.MySqlStorageOptions;

namespace Mango.Core.HangfireScheduler.Extension
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// 添加Hangfire服务端
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IServiceCollection AddMangoHangfire(this IServiceCollection services, string connectionString)
        {
            services.AddHangfire(cf =>
            {
                cf.SetDataCompatibilityLevel(CompatibilityLevel.Version_180);
                cf.UseSimpleAssemblyNameTypeSerializer();
                cf.UseRecommendedSerializerSettings();
                cf.UseStorage(new MySqlStorage(connectionString, new MySqlStorageOptions
                {
                    TransactionIsolationLevel = IsolationLevel.ReadCommitted,
                    QueuePollInterval = TimeSpan.FromSeconds(15),
                    JobExpirationCheckInterval = TimeSpan.FromHours(1),
                    CountersAggregateInterval = TimeSpan.FromMinutes(5),
                    PrepareSchemaIfNecessary = true,
                    DashboardJobListLimit = 50000,
                    TransactionTimeout = TimeSpan.FromMinutes(1),
                }));
                cf.UseConsole();
                cf.UseHangfireHttpJob();
            });
            //服务端
            services.AddHangfireServer();
            return services;
        }

        /// <summary>
        /// 添加Hangfire Agent
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMangoHangfireAgent(this IServiceCollection services)
        {
            services.AddHangfireJobAgent(op =>
            {
                var result = AssemblyHelper.GetAssemblies(x => x.Name.StartsWith("Mango"));

                foreach (var assembly in result)
                {
                    op.AddJobAgent(assembly);
                }
            });
            return services;
        }
    }
}
