using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tocsoft.Common.Helpers.Web
{
    public static class Session
    {
        public static T Get<T>(this System.Web.SessionState.HttpSessionState cache, string key)
        {
            return (T)cache[key];
        }

        public static T Get<T>(string key)
        {
            return HttpContext.Current.Session.Get<T>(key);
        }

        public static T TryGet<T>(string key, Func<T> function) {
            return HttpContext.Current.Session.TryGet<T>(key, function);
        }

        public static T TryGet<T>(this System.Web.SessionState.HttpSessionState cache, string key,Func<T> function)
        {
            var result = cache.Get<T>(key);

            if (result == null)
            {
                //get value to add
                result = function();
                //add to cache
                cache.Add(key, result);
            }
            return result;
        }
    }
}