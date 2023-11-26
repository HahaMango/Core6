/*--------------------------------------------------------------------------
//
//  Copyright 2020 Chiva Chen
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//
--------------------------------------------------------------------------*/

using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Core.Authentication.Policy
{
    /// <summary>
    /// jwt Audience 策略处理程序
    /// </summary>
    public class JwtAudienceHandler : AuthorizationHandler<JwtAudienceRequirement>
    {
        /// <summary>
        /// 处理逻辑
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, JwtAudienceRequirement requirement)
        {
            if (string.IsNullOrEmpty(requirement.VaildAudience))
            {
                throw new ArgumentNullException(nameof(requirement.VaildAudience));
            }

            if(!context.User.HasClaim(item => item.Type == "aud"))
            {
                return Task.CompletedTask;
            }

            if(context.User.Claims.Where(item => item.Type == "aud" && item.Value.Contains(requirement.VaildAudience)).Any())
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
