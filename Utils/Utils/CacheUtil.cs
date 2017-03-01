using System;
using System.Runtime.Caching;

namespace iPms.WebUtilities.Utils
{
    public class CacheUtil
    {
        private static ObjectCache _cache = MemoryCache.Default;

        /// <summary>
        /// 添加缓存（过期时间5分钟）
        /// </summary>
        /// <param name="key">Key 唯一</param>
        /// <param name="value">值</param>
        public static void Add(string key, object value)
        {
            Add(key, value, DateTimeOffset.Now.AddMinutes(5));
        }


        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">Key 唯一</param>
        /// <param name="value">值</param>
        /// <param name="cacheOffset">超时时间</param>
        public static void Add(string key, object value, DateTimeOffset cacheOffset)
        {
            if (_cache.Contains(key))
            {
                Remove(key);
            }

            _cache.Add(key, value, cacheOffset);
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">Key</param>
        public static void Remove(string key)
        {
            if (_cache.Contains(key))
            {
                _cache.Remove(key);
            }
        }

        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns></returns>
        public static object Read(string key)
        {
            if (_cache.Contains(key))
            {
                return _cache[key];
            }

            return null;
        }

    }
}
