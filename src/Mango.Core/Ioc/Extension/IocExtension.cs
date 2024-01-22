using Mango.Core.Helper;
using Mango.Core.Ioc.Attribute;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Core.Ioc.Extension
{
    /// <summary>
    /// IOC拓展类
    /// </summary>
    public static class IocExtension
    {
        /// <summary>
        /// 自动服务注入
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AutoDetectService(this IServiceCollection services)
        {
            var assemblies = AssemblyHelper.GetAssemblies(x=>x.Name.StartsWith("Mango"));
            foreach(var assembly in assemblies)
            {
                //对每个程序集的所有带有Service注解的类注入IOC中
                var serviceTypeList = assembly.GetTypes().Where(x => x.CustomAttributes.Any(x => x.AttributeType == typeof(ServiceAttribute)) && x.IsClass == true && x.IsAbstract == false);
                foreach(var serviceType in serviceTypeList)
                {
                    //获取服务所实现的所有接口列表，进行服务暴露
                    var interfaceList = serviceType.GetInterfaces();
                    //获取服务特性
                    var serviceAttribute = (ServiceAttribute)System.Attribute.GetCustomAttribute(serviceType, typeof(ServiceAttribute));
                    //如果没有实现接口则直接注入
                    if (interfaceList == null || interfaceList.Count() <= 0)
                    {
                        switch (serviceAttribute.LifeTime)
                        {
                            case ServiceLifeTimeEnum.Singleton:
                                services.AddSingleton(serviceType);
                                break;
                            case ServiceLifeTimeEnum.Transient:
                                services.AddTransient(serviceType);
                                break;
                            case ServiceLifeTimeEnum.Scoped:
                                services.AddScoped(serviceType);
                                break;
                        }
                    }
                    //如果实现了接口，遍历所有实现的接口一一暴露服务
                    foreach(var interfaceType in interfaceList)
                    {
                        switch (serviceAttribute.LifeTime)
                        {
                            case ServiceLifeTimeEnum.Singleton:
                                services.AddSingleton(interfaceType, serviceType);
                                break;
                            case ServiceLifeTimeEnum.Transient:
                                services.AddTransient(interfaceType, serviceType);
                                break;
                            case ServiceLifeTimeEnum.Scoped:
                                services.AddScoped(interfaceType, serviceType);
                                break;
                        }
                    }
                }
            }
            return services;
        }
    }
}
