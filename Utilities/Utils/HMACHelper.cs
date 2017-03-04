using System;
using System.Security.Cryptography;
using System.Text;

namespace Utilities.Utils
{
    /// <summary>
    /// 签名生成工具
    /// </summary>
    public sealed class HMACHelper
    {
        /// <summary>
        /// 计算SHA-256的HMAC散列
        /// </summary>
        /// <param name="key">密钥</param>
        /// <param name="content">需要计算的内容</param>
        /// <returns>返回哈希值数组</returns>
        public static byte[] ComputeHMAC(string key, string content)
        {
            return ComputeHMAC(Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(content));
        }

        /// <summary>
        /// 计算SHA-256的HMAC散列
        /// </summary>
        /// <param name="key">密钥</param>
        /// <param name="content">需要计算的内容</param>
        /// <returns>返回哈希值数组</returns>
        public static byte[] ComputeHMAC(byte[] key, string content)
        {
            return ComputeHMAC(key, Encoding.UTF8.GetBytes(content));
        }


        /// <summary>
        /// 计算SHA-256的HMAC散列
        /// </summary>
        /// <param name="key">密钥</param>
        /// <param name="contentBytes"></param>
        /// <returns>返回哈希值数组</returns>
        public static byte[] ComputeHMAC(byte[] key, byte[] contentBytes)
        {
            using (HMAC hmac = new HMACSHA256(key))
            {
                return hmac.ComputeHash(contentBytes);
            }
        }
    }
}