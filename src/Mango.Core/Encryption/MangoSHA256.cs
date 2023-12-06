using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Mango.Core.Encryption
{
    /// <summary>
    /// SHA256 散列
    /// </summary>
    public class MangoSHA256
    {
        /// <summary>
        /// 数组到数组散列
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] bytes)
        {
            using(var sha = SHA256.Create())
            {
                return sha.ComputeHash(bytes);
            }
        }

        /// <summary>
        /// 字符串到数组散列
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] Encrypt(string input)
        {
            var bytes = Encoding.ASCII.GetBytes(input);
            return Encrypt(bytes);
        }

        /// <summary>
        /// 数组到字符串散列
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string Encrypt2String(byte[] bytes)
        {
            var sha256Bytes = Encrypt(bytes);
            return Encoding.ASCII.GetString(sha256Bytes);
        }

        /// <summary>
        /// 字符串到字符串散列
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Encrypt2String(string input)
        {
            var sha256Bytes = Encrypt(input);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < sha256Bytes.Length; i++)
            {
                builder.Append(sha256Bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
