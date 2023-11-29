using Mango.EntityFramework.Abstractions;
using Mango.EntityFramework.KeyGenerator;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mango.EntityFramework.BaseEntity
{
    /// <summary>
    /// 雪花算法实体
    /// </summary>
    public class SnowFlakeEntity : IBaseEntity<long>
    {
        private SnowFlakeGenerator _snowFlakeGenerator;

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        /// <summary>
        /// 生成键
        /// </summary>
        public void SetId()
        {
            if(_snowFlakeGenerator == null)
            {
                _snowFlakeGenerator = SnowFlakeGenerator.Instance();
            }
            Id = _snowFlakeGenerator.GetKey();
        }
    }
}
