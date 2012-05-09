using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tocsoft.Common.Helpers
{
    public static class NumberExtentions
    {
        /// <summary>
        /// An optimized method using an array as buffer instead of
        /// string concatenation. This is faster for return values having
        /// a length > 1.
        /// </summary>
        public static string DecimalToString(this decimal value, string baseChars)
        {
            // 32 is the worst cast buffer size for base 2 and int.MaxValue
            int i = 128;
            char[] buffer = new char[i];
            int targetBase = baseChars.Length;

            do
            {
                buffer[--i] = baseChars[(int)(value % targetBase)];
                value = Math.Floor(value / targetBase);
            }
            while (value > 0);

            char[] result = new char[128 - i];
            Array.Copy(buffer, i, result, 0, 128 - i);

            return new string(result);
        }

        const string defaultBaseChars = "0123456789ABCDEFHJKMNPRTWXZ";

        public static string DecimalToString(this decimal value)
        {
            return value.DecimalToString(defaultBaseChars);
        }

        public static decimal StringToDecimal(this string value)
        {
            return value.StringToDecimal(defaultBaseChars);
        }

        public static decimal StringToDecimal(this string value, string baseChars)
        {
            var len = value.Length;
            decimal total = 0;
            while (0 < len--)
            {
                var pos = (value.Length - len) - 1;
                var c = value[len];
                var c_val = baseChars.IndexOf(c);
                if (pos == 0)
                    total = c_val;
                else
                    total += ((decimal)Math.Pow(baseChars.Length, pos) * c_val);
            }

            return total;
        }

        /// <summary>
        /// An optimized method using an array as buffer instead of
        /// string concatenation. This is faster for return values having
        /// a length > 1.
        /// </summary>
        public static string IntToString(this int value, string baseChars)
        {
            return ((decimal)value).DecimalToString(baseChars);
        }

        public static string IntToString(this int value)
        {
            return ((decimal)value).DecimalToString();
        }

        public static int StringToInt(this string value, string baseChars)
        {
            return (int)value.StringToDecimal(baseChars);
        }

        public static int StringToInt(this string value)
        {
            return (int)value.StringToDecimal();
        }

        /// <summary>
        /// An optimized method using an array as buffer instead of
        /// string concatenation. This is faster for return values having
        /// a length > 1.
        /// </summary>
        public static string LongToString(this long value, string baseChars)
        {
            return ((decimal)value).DecimalToString(baseChars);
        }

        public static string LongToString(this long value)
        {
            return ((decimal)value).DecimalToString();
        }

        public static long StringToLong(this string value, string baseChars)
        {
            return (long)value.StringToDecimal(baseChars);
        }

        public static long StringToLong(this string value)
        {
            return (long)value.StringToDecimal();
        }
    }
}