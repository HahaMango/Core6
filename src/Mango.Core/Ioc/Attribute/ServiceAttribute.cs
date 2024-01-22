using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Core.Ioc.Attribute
{
    /// <summary>
    /// 表示添加到IOC容器的服务
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class ServiceAttribute : System.Attribute
    {
        public ServiceLifeTimeEnum LifeTime { get; set; } = ServiceLifeTimeEnum.Scoped;

        public ServiceAttribute() { }
    }

    public enum ServiceLifeTimeEnum
    {
        /// <summary>
        /// 单例
        /// </summary>
        Singleton,
        
        /// <summary>
        /// 范围（默认）
        /// </summary>
        Scoped,

        /// <summary>
        /// 瞬时
        /// </summary>
        Transient
    }
}
