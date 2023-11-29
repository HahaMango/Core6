using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.KeyGenerator.Abstractions
{
    /// <summary>
    /// 键生成器接口
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IGenerator<TKey>
    {
        /// <summary>
        /// 生成键
        /// </summary>
        /// <returns></returns>
        TKey GetKey();
    }
}
