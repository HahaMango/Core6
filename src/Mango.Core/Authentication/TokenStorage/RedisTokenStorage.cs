using FreeRedis;
using Mango.Core.Authentication.TokenStorage.Abstractions;
using Mango.Core.Config;
using Mango.Core.Serialization.Extension;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Core.Authentication.TokenStorage
{
    /// <summary>
    /// 默认的redis存储实现
    /// </summary>
    internal class RedisTokenStorage : ITokenStorage
    {
        public RedisTokenStorage(string connectionString)
        {
            RedisSingleton.ConnectionString = connectionString;
        }

        public async Task DelAsync(string key)
        {
            await RedisSingleton.Instance.DelAsync(key);
        }
        
        public async Task<IDictionary<string, string>> GetTokenAsync(string key)
        {
            return await RedisSingleton.Instance.GetAsync<IDictionary<string, string>>(key);
        }

        public async Task SaveToStorageAsync(string key, IDictionary<string, string> claims, int timeoutSeconds = 0)
        {
            await RedisSingleton.Instance.SetAsync(key, claims, timeoutSeconds);
        }

        /// <summary>
        /// redis单例
        /// </summary>
        class RedisSingleton : RedisClient
        {
            public static string? ConnectionString { get; set; }
            private static readonly Lazy<RedisSingleton> _instance = new(() =>
            {
                return new RedisSingleton(ConnectionString)
                {
                    Serialize = obj => obj.ToJson(),
                    Deserialize = (obj, type) => obj.ToObject(type)
                };
            });

            private RedisSingleton(string connectionString):base(connectionString)
            {
            }

            public static RedisSingleton Instance
            {
                get
                {
                    ArgumentNullException.ThrowIfNull(ConnectionString);

                    return _instance.Value;
                }
            }
        }
    }
}
