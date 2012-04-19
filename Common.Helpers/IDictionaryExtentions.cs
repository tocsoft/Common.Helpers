using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Helpers
{
    public static class IDictionaryExtentions
    {
        public static TValue GetDefault<TKey, TValue>(this IDictionary<TKey, TValue> values, TKey key)
        {
            return values.GetDefault(key, default(TValue));
        }

        public static TValue GetDefault<TKey, TValue>(this IDictionary<TKey, TValue> values, TKey key, TValue defaultValue)
        {
            if (values.ContainsKey(key))
            {
                return values[key];
            }

            return defaultValue;
        }
    }
}