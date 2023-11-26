using Mango.Core.Serialization.Extension;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Core.Network.Abstractions
{
    /// <summary>
    /// 处理行封装抽象TCP服务端（不能被直接实例化）
    /// 具体应用类需要继承该类，实现HandleBusiness来处理自己的业务
    /// </summary>
    public abstract class HandlePackageLineTcpServer<T> : LineAbstractTcpServer
        where T : DataPackage , new()
    {

        public override async ValueTask<Memory<byte>> Handle(ReadOnlyMemory<byte> input)
        {
            var requestString = Encoding.Unicode.GetString(input.ToArray());
            var jsonObject = await requestString.ToObjectAsync<T>();
            _logger.LogInformation($"[{DateTime.Now}][id:{jsonObject.Id}]: start handle...]");
            var businessResult = await HandleBusiness(jsonObject.Data);
            var response = new T();
            response.Id = jsonObject.Id;
            response.Data = businessResult.ToArray();
            return Encoding.Unicode.GetBytes(response.ToJson());
        }

        protected abstract ValueTask<Memory<byte>> HandleBusiness(ReadOnlyMemory<byte> input);
    }
}
