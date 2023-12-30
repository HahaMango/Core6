using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Mango.Core.Network.WebSocket.Abstraction.Extensions
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// 添加FIWebSocket依赖注入
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddFIWebSocket(this IServiceCollection services, string url, Action<MangoWebSocketOption> option) 
        {
            var op = new MangoWebSocketOption();
            option(op);
            services.AddSingleton((sp) =>
            {
                var logger = sp.GetRequiredService<ILogger<MangoWebSocket>>();
                var sf = sp.GetRequiredService<IServiceScopeFactory>();
                var ws = new MangoWebSocket(logger, sf, new Uri(url), op);
                return ws;
            });
            //注入handler
            if (op.ReceviceHandler != null)
            {
                foreach (var type in op.ReceviceHandler)
                {
                    services.AddScoped(type);
                }
            }
            return services;
        }
    }
}
