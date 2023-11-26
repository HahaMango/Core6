using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Core.DataStructure
{
    /// <summary>
    /// 代表单个微服务实体信息
    /// </summary>
    public class MangoService
    {
        /// <summary>
        /// 服务Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 服务IP地址
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 服务端口
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; set; }
    }
}
