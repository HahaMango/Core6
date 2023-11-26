﻿using Mango.Core.KeyGenerator.Abstractions;
using Snowflake.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Core.KeyGenerator
{
    /// <summary>
    /// 雪花Id生成器，必须为单例
    /// </summary>
    public class SnowFlakeGenerator : IGenerator<long>
    {
        /// <summary>
        /// 雪花生成器（必须为单例，否则可能导致重复键值）
        /// </summary>
        private readonly IdWorker _idWorker;

        private static SnowFlakeGenerator _SnowFlakeGenerator;

        /// <summary>
        /// 防止被实例化
        /// </summary>
        private SnowFlakeGenerator()
        {
            _idWorker = new IdWorker(1, 1);
        }

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <returns></returns>
        public static SnowFlakeGenerator Instance()
        {
            if(_SnowFlakeGenerator == null)
            {
                _SnowFlakeGenerator = new SnowFlakeGenerator();
            }
            return _SnowFlakeGenerator;
        }

        /// <summary>
        /// 生成键
        /// </summary>
        /// <returns></returns>
        public long GetKey()
        {
            return _idWorker.NextId();
        }
    }
}
