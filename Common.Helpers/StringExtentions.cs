using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Helpers
{
    public static class StringExtentions
    {
        public static string UrlEncode(this string str) {
            return Uri.EscapeDataString(str);
        }
    }
}
