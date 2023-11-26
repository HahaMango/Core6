using System.Threading.Tasks;

namespace Mango.Core.HttpService
{
    /// <summary>
    /// json Http服务接口
    /// </summary>
    public interface IJsonHttpService<T> : IBaseHttp
        where T : class, new()
    {
        /// <summary>
        /// get请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="token">jwt令牌</param>
        /// <returns></returns>
        Task<HttpResponse<T>> GetAsync(string url, string token = null);

        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <param name="token">jwt令牌</param>
        /// <returns></returns>
        Task<HttpResponse<T>> PostAsync(string url, string content, string token = null);
    }
}
