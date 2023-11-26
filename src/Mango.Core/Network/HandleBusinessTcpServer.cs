using Mango.Core.Network.Abstractions;
using System;
using System.Threading.Tasks;

namespace Mango.Core.Network
{
    public class HandleBusinessTcpServer<T> : HandlePackageLineTcpServer<T>
        where T : DataPackage, new()
    {
        protected override async ValueTask<Memory<byte>> HandleBusiness(ReadOnlyMemory<byte> input)
        {
            //var s = Encoding.UTF8.GetString(input.ToArray());
            //var r = Encoding.UTF8.GetBytes(s);
            return await Task.FromResult(input.ToArray());
        }
    }
}
