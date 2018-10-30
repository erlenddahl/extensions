using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public static class NullableExtensions
    {
        public static T Safe<T>(this T? item, T defaultValue = default(T)) where T : struct
        {
            if (item.HasValue) return item.Value;
            return defaultValue;
        }
    }
}
