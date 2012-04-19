using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Common.Helpers.Web
{
    public static class RequestExtentions
    {
        public static TValue GetAs<TValue>(this HttpRequestBase request, string key)
        {
            return request[key].As<TValue>();
        }
    }
}