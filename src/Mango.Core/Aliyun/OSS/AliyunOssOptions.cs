using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Core.Aliyun.OSS
{
    /// <summary>
    /// 阿里云OSS配置类（初始化时读取）
    /// </summary>
    public class AliyunOssOptions
    {
        public const string OSS = "OSS";

        /// <summary>
        /// Endpoint
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// AccessKeyId
        /// </summary>
        public string AccessKeyId { get; set; }

        /// <summary>
        /// AccessKeySecret
        /// </summary>
        public string AccessKeySecret { get; set; }

        /// <summary>
        /// BucketName
        /// </summary>
        public string BucketName { get; set; }
    }
}
