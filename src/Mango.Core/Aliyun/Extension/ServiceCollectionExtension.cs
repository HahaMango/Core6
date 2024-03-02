using Aliyun.OSS;
using Mango.Core.Aliyun.OSS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Core.Aliyun.Extension
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// 注入阿里云OssClient
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddAliyunOSS(this IServiceCollection services, ConfigurationManager configuration)
        {
            var section = configuration.GetSection(AliyunOssOptions.OSS);
            var options = section.Get<AliyunOssOptions>();
            services.Configure<AliyunOssOptions>(section);
            services.AddSingleton(sp =>
            {
                return new OssClient($"https://{options.Endpoint}", options.AccessKeyId, options.AccessKeySecret);
            });
            services.AddScoped<AliyunOssApi>();
            return services;
        }
    }
}
