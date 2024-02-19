using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mango.EntityFramework.BaseEntity
{
    public abstract class TimeFieldEntity : Entity
    {
        public override void SetId()
        {
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建自
        /// </summary>
        [Column(TypeName = "varchar(20)")]
        public string? CreateBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 更新自
        /// </summary>
        [Column(TypeName = "varchar(20)")]
        public string? UpdateBy { get; set; }
    }
}
