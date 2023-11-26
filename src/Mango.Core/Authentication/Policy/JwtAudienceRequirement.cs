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
using System.Collections.Generic;
using System.Text;

namespace Mango.Core.Authentication.Policy
{
    /// <summary>
    /// jwt Audience策略要求
    /// </summary>
    public class JwtAudienceRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// 有效的Audience
        /// </summary>
        public string VaildAudience { get; set; }

        /// <summary>
        /// jwt Audience策略要求
        /// </summary>
        /// <param name="vaildAudience"></param>
        public JwtAudienceRequirement(string vaildAudience)
        {
            VaildAudience = vaildAudience;
        }
    }
}
