using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using Extensions.IEnumerableExtensions;

namespace Extensions.ObjectExtensions
{
    public static class UnitTestExtensions
    {
        public static string WriteUnitTests(this object o, string name, bool ignoreNulls = true, bool ignoreObjects = true, double doublePrecision = 0.00001, bool useShortenedStringAssertion = true)
        {
            //Enumerate fields and properties, write Assert.AreEqual.
            //CollectionAssert for primitive arrays?
            //0.00001 for doubles (parameter?)

            var sb = new StringBuilder();
            foreach (var p in o.GetType().GetProperties())
            {
                if (ignoreNulls && p.GetValue(o) == null) continue;
                if (ignoreObjects && !p.GetValue(o).IsSimpleType()) continue;
                sb.AppendLine(GetAssertion(name, p.Name, p.PropertyType, p.GetValue(o), doublePrecision, useShortenedStringAssertion));
            }

            foreach (var p in o.GetType().GetFields())
            {
                if (ignoreNulls && p.GetValue(o) == null) continue;
                if (ignoreObjects && !p.GetValue(o).IsSimpleType()) continue;
                sb.AppendLine(GetAssertion(name, p.Name, p.FieldType, p.GetValue(o), doublePrecision, useShortenedStringAssertion));
            }

            return sb.ToString();
        }

        private static string GetAssertion(string objectName, string propertyName, Type propertyType, object value, double doublePrecision, bool useShortenedStringAssertion)
        {
            if (value is string s)
            {
                if (useShortenedStringAssertion && s.Length > 201)
                {
                    return $"Assert.AreEqual({s.Length}, {objectName}.{propertyName}.Length);" + Environment.NewLine +
                           $"Assert.IsTrue({objectName}.{propertyName}.StartsWith(\"{s.Substring(0, 100).Replace("\"", "\\\"")}\"));" + Environment.NewLine +
                           $"Assert.IsTrue({objectName}.{propertyName}.EndsWith(\"{s.Substring(s.Length - 100, 100).Replace("\"", "\\\"")}\"));";
                }

                value = "\"" + s.Replace("\"", "\\\"") + "\"";
            }

            var prefix = "";
            if (value is ushort) prefix = "(ushort)";
            if (value is uint) prefix = "(uint)";
            if (value is ulong) prefix = "(ulong)";
            if (value is decimal) prefix = "(decimal)";
            return $"Assert.AreEqual({prefix}{(value is double d ? d.ToString(CultureInfo.InvariantCulture) : value)}, {objectName}.{propertyName}{(propertyType == typeof(double) ? $", {doublePrecision.ToString(CultureInfo.InvariantCulture)}" : "")});";
        }
    }
}
