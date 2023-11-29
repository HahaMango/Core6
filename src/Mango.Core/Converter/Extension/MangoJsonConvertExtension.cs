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
            builder.AddJsonOptions(options =>
            {
                var converters = options.JsonSerializerOptions.Converters;
                converters.Add(new DateTimeConverter());
                converters.Add(new IntConverter());
                converters.Add(new LongConverter());
                converters.Add(new NullableDateTimeConverter());
                converters.Add(new NullableIntConverter());
                converters.Add(new NullableLongConverter());
            });
            return builder;
        }
    }
}
