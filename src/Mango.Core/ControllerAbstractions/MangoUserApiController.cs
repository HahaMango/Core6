using Mango.Core.DataStructure;
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
    public class MangoUserApiController : MangoBaseApiController
    {
        /// <summary>
        /// 获取用户信息（没有授权返回为空）
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Claim>? GetClaims()
        {
            if(User == null)
            {
                return null;
            }
            if (!User.Identity.IsAuthenticated)
            {
                return null;
            }
            return User.Claims;
        }
    }
}
