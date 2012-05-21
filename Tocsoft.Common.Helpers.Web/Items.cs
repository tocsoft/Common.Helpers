using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tocsoft.Common.Helpers.Web
{
    public static class Items
    {
        public static T Get<T>(string key)
        {
            return HttpContext.Current.Items.Get<T>(key);
        }

        public static T TryGet<T>(string key, Func<T> function) {
            return HttpContext.Current.Items.TryGet<T>(key, function);
        }
    }
}