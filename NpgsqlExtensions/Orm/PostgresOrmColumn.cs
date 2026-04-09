using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Extensions;

namespace NpgsqlExtensions.Orm
{
    public class PostgresOrmColumn
    {

        public string Name { get; set; }
        public bool IsIdColumn { get; set; }
        public string TypeName { get; set; }
        public Type PropertyType { get; set; }

        public bool CanSet { get; set; }

        public string StaticValue { get; set; } = null;
        public string QueryValue => StaticValue ?? "@" + Name;

        private readonly PropertyInfo _property;

        public PostgresOrmColumn(PropertyInfo property)
        {
            _property = property;
            PropertyType = _property.PropertyType;
            Name = property.Name.ToCamelCase();
            IsIdColumn = property.Name.ToLower() == "id";
            TypeName = GetTypeName(PropertyType);
            CanSet = property.SetMethod != null;
        }

        public PostgresOrmColumn(string name, Type type)
        {
            Name = name;
            PropertyType = type;

            if (type != null)
                TypeName = GetTypeName(type);
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

        public static string GetCTypeName(string dbType)
        {
            if (dbType == "text") return "string";
            if (dbType == "integer") return "int";
            if (dbType == "bigint") return "long";
            if (dbType == "boolean") return "bool";
            if (dbType == "smallint") return "short";
            if (dbType == "real") return "float";
            if (dbType == "double precision") return "double";
            if (dbType == "money") return "decimal";
            if (dbType == "timestamp") return "DateTime";
            if (dbType == "timestamp") return "TimeSpan";

            throw new NotImplementedException("C# type from Postgres object of type " + dbType + " is not implemented.");
        }

        internal void SetValue<T>(T element, object value)
        {
            _property.SetValue(element, value);
        }
    }

    public class PostgresOrmCsvColumn : PostgresOrmColumn
    {
        public int Index { get; }
        public object DefaultValue { get; set; }
        public string ParseFormat { get; set; } = null;
        public CultureInfo ParseCulture { get; set; } = CultureInfo.InvariantCulture;

        public Action<UnnestInserter.UnnestInserter, List<CsvRow>> CustomInsertAdder { get; set; } = null;

        public PostgresOrmCsvColumn(string name, int index, Type type) : base(name, type)
        {
            Index = index;
        }

        public object GetValue(CsvRow element)
        {
            var value = element[Index];

            if (PropertyType == typeof(string)) return value;
            if (PropertyType == typeof(short)) return short.TryParse(value, out var res) ? res : DefaultValue ?? default(short);
            if (PropertyType == typeof(int)) return int.TryParse(value, out var res) ? res : DefaultValue?? default(int);
            if (PropertyType == typeof(long)) return long.TryParse(value, out var res) ? res : DefaultValue ?? default(long);
            if (PropertyType == typeof(double)) return double.TryParse(value, NumberStyles.Any, ParseCulture, out var res) ? res : DefaultValue ?? default(double);
            if (PropertyType == typeof(float)) return float.TryParse(value, NumberStyles.Any, ParseCulture, out var res) ? res : DefaultValue ?? default(float);
            if (PropertyType == typeof(decimal)) return decimal.TryParse(value, NumberStyles.Any, ParseCulture, out var res) ? res : DefaultValue ?? default(decimal);
            if (PropertyType == typeof(bool)) return value.ToLower().StartsWithAny("y", "1", "t");

            if (PropertyType == typeof(DateTime))
            {
                if(string.IsNullOrWhiteSpace(ParseFormat))
                    return DateTime.TryParse(value, out var res) ? res : DefaultValue ?? DateTime.MinValue;
                else
                    return DateTime.TryParseExact(value, ParseFormat, ParseCulture, DateTimeStyles.None, out var res) ? res : DefaultValue ?? DateTime.MinValue;
            }

            if (PropertyType == typeof(TimeSpan))
            {
                if (string.IsNullOrWhiteSpace(ParseFormat))
                    return TimeSpan.TryParse(value, out var res) ? res : DefaultValue ?? TimeSpan.MinValue;
                else
                    return TimeSpan.TryParseExact(value, ParseFormat, ParseCulture, TimeSpanStyles.None, out var res) ? res : DefaultValue ?? TimeSpan.MinValue;
            }

            throw new Exception("Invalid CSV type: " + PropertyType);
        }

        public void AddToInserter(UnnestInserter.UnnestInserter inserter, List<CsvRow> subset)
        {
            if (CustomInsertAdder != null)
            {
                CustomInsertAdder(inserter, subset);
                return;
            }
            inserter.Add(Name, PropertyType, subset.Select(GetValue));
        }
    }

    public class PostgresOrmCsvColumnCollection
    {
        private PostgresOrmCsvColumn[] _columns;

        public PostgresOrmCsvColumnCollection(IEnumerable<string> headers)
        {
            _columns = headers.Select((p, i) => new PostgresOrmCsvColumn(p.Trim().ToLower(), i, null)).ToArray();
        }

        public PostgresOrmCsvColumn[] Build()
        {
            var notSet = _columns.Where(p => p.PropertyType == null);
            if (notSet.Any()) throw new Exception("The following columns have no type set: " + string.Join(", ", notSet.Select(p => p.Name)));

            return _columns;
        }

        public PostgresOrmCsvColumnCollection String(params string[] headers)
        {
            return SetType(typeof(string), headers);
        }

        public PostgresOrmCsvColumnCollection Short(params string[] headers)
        {
            return SetType(typeof(short), headers);
        }

        public PostgresOrmCsvColumnCollection Int(params string[] headers)
        {
            return SetType(typeof(int), headers);
        }

        public PostgresOrmCsvColumnCollection Long(params string[] headers)
        {
            return SetType(typeof(long), headers);
        }

        public PostgresOrmCsvColumnCollection Float(params string[] headers)
        {
            return SetType(typeof(float), headers);
        }

        public PostgresOrmCsvColumnCollection Double(params string[] headers)
        {
            return SetType(typeof(double), headers);
        }

        public PostgresOrmCsvColumnCollection Decimal(params string[] headers)
        {
            return SetType(typeof(Decimal), headers);
        }

        public PostgresOrmCsvColumnCollection Bool(params string[] headers)
        {
            return SetType(typeof(bool), headers);
        }

        public PostgresOrmCsvColumnCollection DateTime(params string[] headers)
        {
            return SetType(typeof(DateTime), headers);
        }

        public PostgresOrmCsvColumnCollection TimeSpan(params string[] headers)
        {
            return SetType(typeof(TimeSpan), headers);
        }

        public PostgresOrmCsvColumnCollection Remove(params string[] headers)
        {
            _columns = _columns.Where(p => !headers.Contains(p.Name)).ToArray();
            return this;
        }

        private PostgresOrmCsvColumnCollection SetType(Type type, params string[] headers)
        {
            return Set(p =>
            {
                p.PropertyType = type;
                p.TypeName = PostgresOrmColumn.GetTypeName(type);
            }, headers);
        }

        public PostgresOrmCsvColumnCollection Set(Action<PostgresOrmCsvColumn> setter, params string[] headers)
        {
            foreach (var c in _columns)
            {
                if (headers.Contains(c.Name, StringComparer.InvariantCultureIgnoreCase))
                {
                    setter(c);
                }
            }

            return this;
        }
    }
}