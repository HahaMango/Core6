using FreeRedis;
using Mango.Core.Cache.Config;
using Mango.Core.Serialization.Extension;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Core.Cache.Extension
{
    /// <summary>
    /// 添加缓存依赖注入
    /// </summary>
    public static class MangoCacheExtension
    {
        /// <summary>
        /// 添加FreeRedisClient
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddFreeRedis(this IServiceCollection services, Action<MangoRedisOptions> options)
        {
            var op = new MangoRedisOptions();
            options(op);
            var csb = new ConnectionStringBuilder[op.Sentinels?.Length ?? 0];
            for (var i = 0; i < csb.Length; i++)
            {
                csb[i] = op.Sentinels[i];
            }
            var client = new RedisClient(op.ConnectionString, csb);
            client.Serialize = obj => obj.ToJson();
            client.Deserialize = (obj, type) => obj.ToObject(type, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            services.AddSingleton<RedisClient>(client);
            return services;
        }
    }
}
