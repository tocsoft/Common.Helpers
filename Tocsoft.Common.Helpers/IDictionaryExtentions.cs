using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tocsoft.Common.Helpers
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

        public static T Get<T>(this IDictionary dict, string key)
        {
            if (dict.Contains(key))
                return (T)dict[key];
            else
                return default(T);
        }

        public static T TryGet<T>(this IDictionary dict, string key, Func<T> function)

        {
            var result = dict.Get<T>(key);

            if (result == null)
            {
                //get value to adds
                result = function();
                //add to cache
                dict.Add(key, result);
            }
            return result;
        }
    }
}