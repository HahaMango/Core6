using Mango.Core.ApiResponse;
using Mango.Core.Authentication.TokenStorage;
using Mango.Core.Authentication.TokenStorage.Abstractions;
using Mango.Core.Enums;
using Mango.Core.Serialization.Extension;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Mango.Core.Authentication.Scheme
{
    /// <summary>
    /// 自定义的认证处理器（未授权和未认证均返回json格式的错误信息）
    /// 
    /// 可以继承该类
    /// </summary>
    public class JsonResultJwtAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        /// <summary>
        /// 方案名称
        /// </summary>
        public const string SchemeName = "mango_jwt";

        public JsonResultJwtAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            //执行默认的jwt认证
            var result = await Context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
            //尝试获取ITokenStorage对象
            var ts = Context.RequestServices.GetService<ITokenStorage>();
            if(ts != null)
            {
                //如果ITokenStorage不为空，则校验ITokenStorage中是否存在该token
                //检查result中是否有claim列表
                var claims = result.Principal?.Claims;
                if (claims != null)
                {
                    //如果claims不为空，则判断列表里面是否包含uuid的claim
                    var uuidCalim = claims.FirstOrDefault(x => x.Type == "uuid");
                    if (uuidCalim != null)
                    {
                        //判断uuid是否在对应的Storage中有效
                        var key = KeyConfig.GetTokenKey(uuidCalim.Value);
                        var storageValue = await ts.GetTokenAsync(key);
                        if(storageValue == null)
                        {
                            //如果不存在，则报错
                            return AuthenticateResult.Fail("该操作需要授权登录后才能继续进行");
                        }
                    }
                }
            }
            return result;
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.ContentType = "application/json";
            Response.StatusCode = StatusCodes.Status401Unauthorized;
            await Response.WriteAsync(new ApiResult(Code.Unauthorized,
                   "该操作需要授权登录后才能继续进行").ToJson());
        }

        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.ContentType = "application/json";
            Response.StatusCode = StatusCodes.Status403Forbidden;
            await Response.WriteAsync(new ApiResult(Code.Forbidden,
                   "当前用户权限不足，不能继续执行").ToJson());
        }
    }
}
