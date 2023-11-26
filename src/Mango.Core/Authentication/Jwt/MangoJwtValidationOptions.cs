using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Core.Authentication.Jwt
{
    /// <summary>
    /// jwt认证配置
    /// </summary>
    public class MangoJwtValidationOptions
    {
        /// <summary>
        /// 加密密钥
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 域
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 颁发机构
        /// </summary>
        public string Issuer { get; set; }
    }
}
