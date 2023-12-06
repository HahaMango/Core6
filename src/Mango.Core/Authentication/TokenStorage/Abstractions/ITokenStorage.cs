using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Core.Authentication.TokenStorage.Abstractions
{
    /// <summary>
    /// 认证token的存储接口
    /// </summary>
    public interface ITokenStorage
    {
        /// <summary>
        /// 获取token信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<IDictionary<string, string>> GetTokenAsync(string key);

        /// <summary>
        /// 设置token信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        Task SaveToStorageAsync(string key, IDictionary<string, string> loginInfo, int timeoutSeconds);

        /// <summary>
        /// 删除token
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task DelAsync(string key);
    }
}
