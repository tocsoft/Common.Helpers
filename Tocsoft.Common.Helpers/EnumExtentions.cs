using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tocsoft.Common.Helpers
{
    public static class EnumExtentions
    {
        public static IEnumerable<T> GetList<T>() where T : struct
        {
            return Enum.GetValues(typeof(T)).Cast<int>().Distinct().Cast<T>();

        }
    }
}
