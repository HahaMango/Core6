using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Core.Config
{
    /// <summary>
    /// 配置文件帮助类
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        /// 根据配置文件名获取IConfiguration对象
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static IConfiguration GetConfigurationByJson(string fileName)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile(fileName, false, false);
            IConfigurationRoot root = builder.Build();
            return root;
        }
    }
}
