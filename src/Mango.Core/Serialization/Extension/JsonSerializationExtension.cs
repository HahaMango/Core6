using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Mango.Core.Converter;

namespace Mango.Core.Serialization.Extension
{
    /// <summary>
    /// JSON序列化扩展类
    /// </summary>
    public static class JsonSerializationExtension
    {
        private static JsonSerializerOptions _options;

        static JsonSerializationExtension()
        {
            _options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            _options.Converters.Add(new DateTimeConverter());
            _options.Converters.Add(new IntConverter());
            _options.Converters.Add(new LongConverter());
            _options.Converters.Add(new NullableDateTimeConverter());
            _options.Converters.Add(new NullableIntConverter());
            _options.Converters.Add(new NullableLongConverter());
        }

        /// <summary>
        /// 对象转json字符串 异步
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        public static async Task<string> ToJsonAsync<T>(this T o)
            where T : class
        {
            return await o.ToJsonAsync<T>(_options);
        }

        /// <summary>
        /// 对象转json字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T o)
            where T : class
        {
            return o.ToJson<T>(_options);
        }

        /// <summary>
        /// 对象转utf8格式json字符传
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string ToJsonUtf8<T>(this T o)
            where T : class
        {
            return o.ToJsonUtf8<T>(_options);
        }

        /// <summary>
        /// 通过配置，对象转json字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T o, JsonSerializerOptions options)
            where T : class
        {
            if (o == null)
            {
                throw new ArgumentNullException(nameof(o));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return JsonSerializer.Serialize<T>(o, options);
        }

        /// <summary>
        /// 通过配置，对象转utf8格式json字符传
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string ToJsonUtf8<T>(this T o, JsonSerializerOptions options)
            where T : class
        {
            if (o == null)
            {
                throw new ArgumentNullException(nameof(o));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var utf8Bytes = JsonSerializer.SerializeToUtf8Bytes<T>(o, options);
            return Encoding.UTF8.GetString(utf8Bytes);
        }

        /// <summary>
        /// 通过配置，对象转json字符串 异步
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        public static async Task<string> ToJsonAsync<T>(this T o, JsonSerializerOptions options)
            where T : class
        {
            if (o == null)
            {
                throw new ArgumentNullException(nameof(o));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            string json = null;

            using (var steam = new MemoryStream())
            {
                await JsonSerializer.SerializeAsync<T>(steam, o);
                using (var reader = new StreamReader(steam))
                {
                    json = await reader.ReadToEndAsync();
                }
            }

            return json;
        }

        /// <summary>
        /// json字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static async Task<T> ToObjectAsync<T>(this string source)
            where T : class, new()
        {
            return await source.ToObjectAsync<T>(_options);
        }

        /// <summary>
        /// 通过配置，json字符串反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static async Task<T> ToObjectAsync<T>(this string source, JsonSerializerOptions options)
            where T : class, new()
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            var o = JsonSerializer.Deserialize<T>(source, options);
            return await Task.FromResult(o);
        }

        /// <summary>
        /// json字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string source)
            where T : class, new()
        {
            return source.ToObject<T>(_options);
        }

        /// <summary>
        /// 通过配置，json字符串反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string source, JsonSerializerOptions options)
            where T : class, new()
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            return JsonSerializer.Deserialize<T>(source, options);
        }

        /// <summary>
        /// json字符串反序列化为对象
        /// </summary>
        /// <param name="source"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ToObject(this string source, Type type)
        {
            return source.ToObject(type, _options);
        }

        /// <summary>
        /// 通过配置，json字符串反序列化对象
        /// </summary>
        /// <param name="source"></param>
        /// <param name="options"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ToObject(this string source, Type type, JsonSerializerOptions options)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            return JsonSerializer.Deserialize(source, type, options);
        }
    }
}
