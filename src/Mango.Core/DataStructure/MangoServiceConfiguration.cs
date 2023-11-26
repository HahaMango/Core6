using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Core.DataStructure
{
    /// <summary>
    /// 预定义服务配置项
    /// </summary>
    public class MangoServiceConfiguration
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string DbConnectString { get; set; }

        /// <summary>
        /// redis连接字符串
        /// </summary>
        public string RedisConnectString { get; set; }

        /// <summary>
        /// JWT密钥
        /// </summary>
        public string JwtKey { get; set; }

        /// <summary>
        /// JWT有效的域
        /// </summary>
        public string ValidAudience { get; set; }

        /// <summary>
        /// JWT有效的颁发机构
        /// </summary>
        public string ValidIssuer { get; set; }
    }
}
