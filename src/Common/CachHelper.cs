using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
//
using System.Runtime.Caching;

namespace Common
{
    /// <summary>
    /// 缓存处理类
    /// </summary>
    public class CachHelper
    {
        private static ObjectCache oCache = MemoryCache.Default;

        /// <summary>
        /// 查询缓存对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Object Query(string name)
        {
            return oCache[name];
        }

        /// <summary>
        /// 添加缓存项
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <param name="seconds"></param>
        public static void Insert(string name, Object value, int seconds)
        {
            CacheItemPolicy oPolicy = new CacheItemPolicy();
            oPolicy.AbsoluteExpiration = DateTime.Now.AddSeconds(seconds);
            oCache.Add(name, value, oPolicy);
        }

        /// <summary>
        /// 移除缓存项
        /// </summary>
        /// <param name="name"></param>
        public static void Remove(string name)
        {
            oCache.Remove(name);
        }

    }
}
