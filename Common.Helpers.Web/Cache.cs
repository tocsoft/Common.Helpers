
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Common.Helpers.Web
{
    public class Cache 
    {
        public Cache() {
        }

        const string PREFIX = "common_helpers_web_";
      

        public static T Get<T>(string key)
        {
            return (T)HttpContext.Current.Cache.Get(PREFIX + key);
        }

        public static T TryGet<T>(string key, Func<T> function) {
            return TryGet(key, new TimeSpan(0, 10, 0), function);
        }
        public static T TryGet<T>(string key, TimeSpan cacheOut, Func<T> function)
        {
            var result = Get<T>(key);

            if (result == null)
            {
                //get value to add
                result = function();
                //add to cache
                HttpContext.Current.Cache.Insert(PREFIX + key, result, null, DateTime.Now.Add(cacheOut), System.Web.Caching.Cache.NoSlidingExpiration);
            }
            return result;
        }


    }

}