namespace Mango.Core.HttpService
{
    /// <summary>
    /// http请求头常量
    /// </summary>
    public static class HttpRequestHeaderConst
    {
        #region accept 头
        /// <summary>
        /// ACCEPT请求头
        /// </summary>
        public static AcceptHeader Accept { get; }

        #endregion

        /// <summary>
        /// content-type请求头
        /// </summary>
        public static ContentTypeHeader ContentType { get; }

        /// <summary>
        /// 认证请求头
        /// </summary>
        public static AuthorizationHeader Authorization { get; }

        static HttpRequestHeaderConst()
        {
            Accept = new AcceptHeader();
            ContentType = new ContentTypeHeader();
            Authorization = new AuthorizationHeader();
        }
    }

    /// <summary>
    /// accept请求头值
    /// </summary>
    public class AcceptHeader
    {
        /// <summary>
        /// 请求头key名称
        /// </summary>
        public readonly string HeaderKeyName = "Accept";

        /// <summary>
        /// html
        /// </summary>
        public readonly string HTML = "text/html";

        /// <summary>
        /// 纯文本
        /// </summary>
        public readonly string TEXT = "text/plain";

        /// <summary>
        /// Json
        /// </summary>
        public readonly string JSON = "application/json";

        /// <summary>
        /// xml
        /// </summary>
        public readonly string XML = "application/xml";
    }

    /// <summary>
    /// content-type请求头
    /// </summary>
    public class ContentTypeHeader : AcceptHeader
    {
        /// <summary>
        /// 请求头key名称
        /// </summary>
        public new readonly string HeaderKeyName = "Content-Type";
    }

    /// <summary>
    /// 认证请求头
    /// </summary>
    public class AuthorizationHeader
    {
        /// <summary>
        /// 请求头key名称
        /// </summary>
        public readonly string HeaderKeyName = "Authorization";

        /// <summary>
        /// Bearer
        /// </summary>
        public readonly string Bearer = "Bearer ";
    }
}
