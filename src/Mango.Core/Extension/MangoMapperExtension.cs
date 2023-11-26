using Nelibur.ObjectMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Core.Extension
{
    /// <summary>
    /// 映射扩展类
    /// </summary>
    public static class MangoMapperExtension
    {
        /// <summary>
        /// 执行映射
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TDestination MapTo<TSource, TDestination>(this object source)
        {
            if(source is not TSource)
            {
                throw new InvalidCastException($"来源类型不正确：{source.GetType().Name}和泛型{typeof(TSource).Name}不匹配");
            }
            if(!TinyMapper.BindingExists<TSource, TDestination>())
            {
                //如果没绑定过，则先绑定
                TinyMapper.Bind<TSource, TDestination>();
            }
            return TinyMapper.Map<TDestination>(source);
        }

        /// <summary>
        /// 列表到列表的映射
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="objects"></param>
        /// <returns></returns>
        public static IEnumerable<TDestination> MapToList<TSource, TDestination>(this IEnumerable<TSource> objects)
        {
            var result = new List<TDestination>();
            foreach(var o in objects)
            {
                result.Add(o.MapTo<TSource, TDestination>());
            }
            return result;
        }
    }
}
