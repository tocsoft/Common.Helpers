using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tocsoft.Common.Helpers.Web
{
    public static class Cache
    {
        public static T Get<T>(this System.Web.Caching.Cache cache, string key)
        {
            return (T)cache.Get(key);
        }

        public static T Get<T>(string key)
        {
            return HttpContext.Current.Cache.Get<T>(key);
        }

        public static T TryGet<T>(string key, Func<T> function) {
            return HttpContext.Current.Cache.TryGet<T>(key, function);
        }

        public static T TryGet<T>(this System.Web.Caching.Cache cache, string key, Func<T> function)
        {
            return cache.TryGet(key, new TimeSpan(0, 10, 0), function);
        }

        public static T TryGet<T>(string key, TimeSpan cacheOut, Func<T> function)
        {
            return HttpContext.Current.Cache.TryGet<T>(key,cacheOut, function);
        }

        public static T TryGet<T>(this System.Web.Caching.Cache cache, string key, TimeSpan cacheOut, Func<T> function)
        {
            var result = Get<T>(key);

            if (result == null)
            {
                //get value to add
                result = function();
                //add to cache
                cache.Insert(key, result, null, DateTime.Now.Add(cacheOut), System.Web.Caching.Cache.NoSlidingExpiration);
            }
            return result;
        }
    }
}