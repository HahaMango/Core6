using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Core.Converter.Extension
{
    /// <summary>
    /// JSON数据类型转换拓展类
    /// </summary>
    public static class MangoJsonConvertExtension
    {
        /// <summary>
        /// 添加常用JSON类型转换
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder AddMangoJsonConvert(this IMvcBuilder builder)
        {
            return builder;
        }
    }
}
