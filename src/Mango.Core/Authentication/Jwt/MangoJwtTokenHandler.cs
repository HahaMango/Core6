using Mango.Core.Authentication.TokenStorage;
using Mango.Core.Authentication.TokenStorage.Abstractions;
using Mango.Core.IDGenerator;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Mango.Core.Authentication.Jwt
{
    /// <summary>
    /// jwt token颁发处理器（过期修改密码等操作由客户端放弃密钥）
    /// </summary>
    public class MangoJwtTokenHandler
    {
        /// <summary>
        /// jwt配置
        /// </summary>
        public MangoJwtOptions Options { get; }

        private readonly ITokenStorage? _tokenStorage;

        /// <summary>
        /// 构造函数
        /// </summary>
        public MangoJwtTokenHandler() 
        {
            Options = new MangoJwtOptions();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="options"></param>
        public MangoJwtTokenHandler(MangoJwtOptions options, ITokenStorage? tokenStorage)
        {
            Options = options;
            _tokenStorage = tokenStorage;
        }

        /// <summary>
        /// 颁发令牌
        /// </summary>
        /// <param name="user"></param>
        /// <param name="otherClaims">额外添加的claims claimType为'aud'可追加aud字段。</param>
        /// <param name="audience">接受者</param>
        /// <param name="issuer">颁发机构</param>
        /// <returns></returns>
        public async Task<string> IssuedTokenAsync(Claim[] claims, string? issuer = null,string? audience = null)
        {
            //生成雪花id，并插入到claim中
            var uuid = SnowFlakeGenerator.Instance.GetKey();
            var cl = new List<Claim>
            {
                new Claim("uuid", uuid.ToString())
            };
            cl.AddRange(claims);

            var sec = Options.ExpiresSec.HasValue ? Options.ExpiresSec.Value : 604800;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Options.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: issuer ?? Options.DefalutIssuer,
                audience: audience ?? Options.DefalutAudience,
                claims: cl,
                expires: DateTime.Now.AddSeconds(sec),
                signingCredentials: creds);

            //如果存在ITokenStorage则把claims储存到对应的storage中
            if(_tokenStorage != null)
            {
                var claimList = new Dictionary<string, string>();
                foreach(var claim in cl)
                {
                    claimList.Add(claim.Type, claim.Value);
                }
                var redisKey = KeyConfig.GetTokenKey(uuid.ToString());
                await _tokenStorage.SaveToStorageAsync(redisKey, claimList, sec);
            }

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// 颁发令牌
        /// </summary>
        /// <typeparam name="TUser"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="user"></param>
        /// <param name="otherClaims"></param>
        /// <returns></returns>
        public async Task<string> IssuedTokenAsync(Claim[] claims)
        {
            return await IssuedTokenAsync(claims : claims, issuer: null, audience: null);
        }
    }
}
