using Mango.Core.Authentication.Jwt;
using Mango.Core.Authentication.Scheme;
using Mango.Core.Authentication.TokenStorage;
using Mango.Core.Authentication.TokenStorage.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Core.Authentication.Extension
{
    /// <summary>
    /// Mango Jwt处理程序的DI扩展类
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// 添加Mango Jwt颁发处理程序到DI
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options">jwt颁发处理器配置</param>
        /// <returns></returns>
        public static IServiceCollection AddMangoJwtHandler(this IServiceCollection services, Action<MangoJwtOptions> options)
        {
            var jwtOptions = new MangoJwtOptions();
            options(jwtOptions);
            services.AddSingleton(sp =>
            {
                using var sc = sp.CreateScope();
                var ts = sc.ServiceProvider.GetService<ITokenStorage>();
                return new MangoJwtTokenHandler(jwtOptions, ts);
            });
            return services;
        }

        /// <summary>
        /// 添加jwt认证
        /// 
        /// 基于Audience字段权限控制，Audience字段形如x.y.z 。如果配置的Audience字段为x.y,则只有具有形如x.y.[z1.z2...zn]的Token才能够认证通过,如token只有x。则无法通过认证。
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options">jwt认证配置</param>
        /// <returns></returns>
        public static IServiceCollection AddMangoJwtAuthentication(this IServiceCollection services,Action<MangoJwtValidationOptions> options)
        {
            var jwtOptions = new MangoJwtValidationOptions();
            options(jwtOptions);
            services.AddAuthentication(JsonResultJwtAuthenticationHandler.SchemeName)
                .AddScheme<AuthenticationSchemeOptions, JsonResultJwtAuthenticationHandler>(JsonResultJwtAuthenticationHandler.SchemeName, op => { })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(15),
                        ValidateIssuerSigningKey = true,
                        ValidAudience = jwtOptions.Audience,
                        ValidIssuer = jwtOptions.Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                    };
                });
            return services;
        }

        /// <summary>
        /// 添加token存储容器服务，可自定义实现ITokenStorage接口即可
        /// </summary>
        /// <typeparam name="ImpTokenStorage"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMangoAuthenticationTokenStorage<ImpTokenStorage>(this IServiceCollection services, Func<IServiceProvider, ImpTokenStorage> op) 
            where ImpTokenStorage : class,ITokenStorage
        {
            services.AddScoped<ITokenStorage, ImpTokenStorage>(op);
            return services;
        }

        /// <summary>
        /// 添加token存储容器服务，可自定义实现ITokenStorage接口即可
        /// </summary>
        /// <typeparam name="ImpTokenStorage"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMangoAuthenticationTokenStorage<ImpTokenStorage>(this IServiceCollection services)
            where ImpTokenStorage : class, ITokenStorage
        {
            services.AddScoped<ITokenStorage, ImpTokenStorage>();
            return services;
        }

        /// <summary>
        /// 添加token存储容器服务，可自定义实现ITokenStorage接口即可
        /// </summary>
        /// <typeparam name="ImpTokenStorage"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMangoRedisAuthenticationTokenStorage(this IServiceCollection services, string redisConnecitonString)
        {
            services.AddMangoAuthenticationTokenStorage(op =>
            {
                return new RedisTokenStorage(redisConnecitonString);
            });
            return services;
        }
    }
}
