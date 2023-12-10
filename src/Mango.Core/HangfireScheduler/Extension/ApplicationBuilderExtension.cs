using Hangfire.Dashboard.BasicAuthorization;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire.HttpJob.Agent.MysqlConsole;

namespace Mango.Core.HangfireScheduler.Extension
{
    public static class ApplicationBuilderExtension
    {
        /// <summary>
        /// 添加hangfire面板
        /// </summary>
        /// <param name="app"></param>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseMangoHangfireDashboard(this IApplicationBuilder app, string loginName, string password)
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-CN");
            app.UseHangfireDashboard(options: new DashboardOptions
            {
                Authorization = new[]
                {
                    new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions
                    {
                        SslRedirect = false,
                        RequireSsl = false,
                        LoginCaseSensitive = true,
                        Users = new []
                        {
                            new BasicAuthAuthorizationUser
                            {
                                Login = loginName,
                                PasswordClear = password
                            }
                        }
                    })
                }
            });
            return app;
        }

        public static IApplicationBuilder UseMangoHangfireAgent(this IApplicationBuilder app, string authName, string password)
        {
            app.UseHangfireJobAgent(op =>
            {
                op.Enabled(true);
                op.WithSitemap("/jobagent");
                op.EnabledBasicAuth(true);
                op.WithBasicUserName(authName);
                op.WithBasicUserPwd(password);
            });
            return app;
        }
    }
}
