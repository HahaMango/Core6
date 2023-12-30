using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Mango.Core.ControllerAbstractions
{
    /// <summary>
    /// 基于用户的基础控制器实现
    /// </summary>
    public abstract class MangoUserApiController : MangoBaseApiController
    {
        /// <summary>
        /// 获取用户信息（没有授权返回为空）
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<Claim>? GetClaims()
        {
            if(User == null || User.Identity == null)
            {
                return null;
            }
            if (!User.Identity.IsAuthenticated)
            {
                return null;
            }
            if(User.Claims == null || !User.Claims.Any())
            {
                return null;
            }
            return User.Claims;
        }
    }
}
