using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Core.Network
{
    /// <summary>
    /// TCP数据包装类
    /// </summary>
    public class DataPackage
    {
        /// <summary>
        /// request Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// data
        /// </summary>
        public byte[] Data { get; set; }
    }
}
