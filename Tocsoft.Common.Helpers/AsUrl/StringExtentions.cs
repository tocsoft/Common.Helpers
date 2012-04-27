using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tocsoft.Common.Helpers.AsUrl
{
    public static class StringExtentions
    {
        public static string AddQueryString(this string str, string name, object value) {
            char sep = '?';
            if (str.Contains(sep)) { sep = '&'; }

            return string.Concat(str, sep, name.UrlEncode(), '=', (value ?? "").ToString().UrlEncode());
        }

        public static string AddQueryString(this string str, params object[] values)
        {
            char sep = '?';
            if (str.Contains(sep)) { sep = '&'; }

            StringBuilder sb = new StringBuilder(str);
            foreach(var dict in values.Select(x=>x.ToDictionary())){
                foreach (var kvp in dict)
                {
                    sb.Append(sep);
                    sb.Append(kvp.Key.UrlEncode());
                    sb.Append("=");
                    sb.Append((kvp.Value ?? "").ToString().UrlEncode());
                    sep= '&';
                }
            }

            return sb.ToString();
        }
    }
}