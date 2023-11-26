using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Core.Cache.Config
{
    /// <summary>
    /// csredis 配置类
    /// </summary>
    public class MangoRedisOptions
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 哨兵地址
        /// </summary>
        public string[] Sentinels { get; set; }
    }
}
