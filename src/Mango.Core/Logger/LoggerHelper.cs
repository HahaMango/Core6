using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Core.Logger
{
    /// <summary>
    /// 日志帮助程序
    /// </summary>
    public class LoggerHelper
    {
        private static ILoggerFactory _factory;

        /// <summary>
        /// 获取日志工厂
        /// </summary>
        public static ILoggerFactory LoggerFactory
        {
            get
            {
                if(_factory == null)
                {
                    _factory = Microsoft.Extensions.Logging.LoggerFactory.Create(build =>
                    {
                        build.AddConsole();
                    });
                }
                return _factory;
            }
            set
            {
                _factory = value;
            }
        }

        /// <summary>
        /// 创建日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ILogger<T> Create<T>()
        {
            return LoggerFactory.CreateLogger<T>();
        }
    }
}
