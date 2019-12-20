using System;
using System.Reflection;
using Extensions;

namespace NpgsqlExtensions.Orm
{
    public class PostgresOrmColumn
    {

        public string Name { get; set; }
        public bool IsIdColumn { get; set; }
        public string TypeName { get; set; }
        public Type PropertyType => _property.PropertyType;

        private readonly PropertyInfo _property;

        public PostgresOrmColumn(PropertyInfo property)
        {
            _property = property;
            Name = property.Name.ToCamelCase();
            IsIdColumn = property.Name.ToLower() == "id";
            TypeName = GetTypeName(property.PropertyType);
        }

        public object GetValue<T>(T element)
        {
            return _property.GetValue(element);
        }

        public static string GetTypeName(Type o)
        {
            if (o == typeof(string)) return "text";
            if (o == typeof(int) || o == typeof(int?)) return "integer";
            if (o == typeof(long) || o == typeof(long?)) return "bigint";
            if (o == typeof(bool) || o == typeof(bool?)) return "boolean";
            if (o == typeof(short) || o == typeof(short?)) return "smallint";
            if (o == typeof(float) || o == typeof(float?)) return "real";
            if (o == typeof(double) || o == typeof(double?)) return "double precision";
            if (o == typeof(decimal) || o == typeof(decimal?)) return "money";
            if (o == typeof(DateTime) || o == typeof(DateTime?)) return "timestamp";
            if (o == typeof(TimeSpan) || o == typeof(TimeSpan?)) return "timestamp";

            throw new NotImplementedException("Postgres type from object of type " + o.Name + " is not implemented.");
        }

        internal void SetValue<T>(T element, object value)
        {
            _property.SetValue(element, value);
        }
    }
}