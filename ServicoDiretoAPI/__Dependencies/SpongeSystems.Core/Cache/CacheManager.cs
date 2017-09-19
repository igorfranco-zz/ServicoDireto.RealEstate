using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;

namespace SpongeSystems.Core.Cache
{
    public class CacheManager
    {
        /// <summary>
        /// busca um item no cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            if(System.Web.HttpContext.Current.Cache[key] != null)
                return (T)System.Web.HttpContext.Current.Cache[key];
            else
                return default(T);
        }
        
        /// <summary>
        /// Insere um novo item no cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Insert<T>(string key, T value)
        {
            System.Web.HttpContext.Current.Cache.Insert(key, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireTime">minutes</param>
        public static void Insert<T>(string key, T value, int expireTime)
        {
            System.Web.HttpContext.Current.Cache.Insert(key, value, null, DateTime.Now.AddMinutes(expireTime), TimeSpan.MinValue);
        }

        /// Insere ou buscar um novo no cache
        public static T GetInsert<T>(string key, Func<T> method)
        {
            T result = Get<T>(key);
            if (result == null || result.Equals(default(T)))
            {
                T value = method.Invoke();
                Insert(key, value);
                result = value;
            }
            return result;
        }

        public static void Remove(string key) 
        {
            if (System.Web.HttpContext.Current.Cache[key] != null)
                System.Web.HttpContext.Current.Cache.Remove(key);
        }
    }
}
