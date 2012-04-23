using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Tocsoft.Common.Helpers.Web
{
    public static class RequestExtentions
    {
        public static TValue GetAs<TValue>(this HttpRequestBase request, string key)
        {
            try
            {
                return request[key].As<TValue>();
            }
            catch { }

            return default(TValue);
        }
    }
}