using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.EntityFramework.Abstractions
{
    /// <summary>
    /// 实体基础接口
    /// </summary>
    public interface IBaseEntity<TKey> : IEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        TKey Id { get; set; }

        /// <summary>
        /// 设置Id
        /// </summary>
        void SetId();
    }
}
