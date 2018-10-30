using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public static class ArgumentExtensions
    {
        public static string GetArgument(this string[] args, string key, string defaultValue = "")
        {
            for (var i = 0; i < args.Length; i++)
                if (args[i] == key && args.Length > i + 1)
                    return args[i + 1];

            return defaultValue;
        }

        public static bool HasSwitch(this string[] args, string key)
        {
            return args.Any(p => p == key);
        }

        public static bool CheckMandatoryArgs(this string[] args, Action<string> missingCallback, params string[] keys)
        {
            var res = true;
            foreach (var key in keys.Where(key => string.IsNullOrEmpty(args.GetArgument(key))))
            {
                res = false;
                missingCallback(key);
            }
            return res;
        }
    }
}
