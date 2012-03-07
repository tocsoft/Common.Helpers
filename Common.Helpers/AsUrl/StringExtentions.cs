using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Helpers.AsUrl
{
    public static class StringExtentions
    {
        public static string AddQueryString(this string str, params object[] values) {

            char sep = '?';
            if (str.Contains(sep)) { sep = '&'; }
            StringBuilder sb = new StringBuilder(str);
            foreach(var dict in values.Select(x=>x.ToDictionary())){

                foreach (var kvp in dict)
                {

                    sb.Append(kvp.Key);
                    sb.Append(sep);
                    sb.Append(kvp.Value.ToString().UrlEncode());
                    sep= '&';

                }
            }

            return sb.ToString();
        }
    }
}
