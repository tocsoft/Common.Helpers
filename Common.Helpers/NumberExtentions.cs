using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Helpers
{
    public static class NumberExtentions
    {
        /// <summary>
        /// An optimized method using an array as buffer instead of 
        /// string concatenation. This is faster for return values having 
        /// a length > 1.
        /// </summary>
        public static string IntToString(this long value, string baseChars)
        {
            // 32 is the worst cast buffer size for base 2 and int.MaxValue
            int i = 32;
            char[] buffer = new char[i];
            int targetBase = baseChars.Length;

            do
            {
                buffer[--i] = baseChars[(int)(value % targetBase)];
                value = value / targetBase;
            }
            while (value > 0);

            char[] result = new char[32 - i];
            Array.Copy(buffer, i, result, 0, 32 - i);

            return new string(result);

        }
        const string defaultBaseChars = "0123456789ABCDEFHJKMNPRTWXZ";
        public static string IntToString(this long value)
        {
            return value.IntToString(defaultBaseChars);
        }

        public static long StringToInt(this string value)
        {
            return value.StringToInt(defaultBaseChars);
        }

        public static long StringToInt(this string value, string baseChars)
        {
            var len = value.Length;
            long total = 0;
            while (0 < len--)
            {
                var pos = (value.Length - len) - 1;
                var c = value[len];
                var c_val = baseChars.IndexOf(c);
                if (pos == 0)
                    total = c_val;
                else
                    total += ((long)Math.Pow(baseChars.Length, pos) * c_val);
            }

            return total;

        }
    }

}
