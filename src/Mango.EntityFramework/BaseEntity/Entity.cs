using Mango.EntityFramework.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mango.EntityFramework.BaseEntity
{
    /// <summary>
    /// 基础实体
    /// </summary>
    public abstract class Entity : IBaseEntity<int>
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// 设置Id
        /// </summary>
        public abstract void SetId();
    }
}
