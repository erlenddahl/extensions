using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Extensions.AssemblyExtensions
{
    public static class Reflection
    {
        public static IEnumerable<Type> GetInheritingTypes(this Type baseType)
        {
            foreach (var type in Assembly.GetAssembly(baseType).GetTypes().Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(baseType)))
            {
                yield return type;
            }
        }
    }
}
