using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mango.Core.Converter
{
    /// <summary>
    /// 可空时间类型JSON转换
    /// </summary>
    public class NullableDateTimeConverter : JsonConverter<DateTime?>
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dateTimeString = reader.GetString();
            if(!string.IsNullOrEmpty(dateTimeString) && !dateTimeString.Contains(':'))
            {
                DateTime dt = DateTime.ParseExact(dateTimeString, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
                return dt;
            }
            if (!string.IsNullOrEmpty(dateTimeString))
            {
                DateTime dt = DateTime.ParseExact(dateTimeString, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
                return dt;
            }

            return default;
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteStringValue(value.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}
