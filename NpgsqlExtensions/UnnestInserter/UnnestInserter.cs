using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;

namespace NpgsqlExtensions.UnnestInserter
{
    public class UnnestInserter
    {
        public readonly List<IUnnestableColumn> UnnestColumns = new List<IUnnestableColumn>();
        public int? InsertTimeout { get; set; }
        private readonly NpgsqlCommandBuilder _cmdBuilder;
        private IEnumerable<IUnnestableColumn> AllColumns => UnnestColumns;

        public string TableName { get; set; }

        /// <summary>
        /// If set to true, the table and column names will be quoted. This is useful if it's an identifier or multiple words,
        /// but the casing must be correct (usually all lower case) when quoted.
        /// </summary>
        public bool QuoteNames { get; set; }

        public string InsertTemplate { get; set; } = "INSERT INTO {0}({1}) VALUES({2})";

        public UnnestInserter(string tableName)
        {
            TableName = tableName;
            _cmdBuilder = new NpgsqlCommandBuilder();
        }

        public void Insert(NpgsqlConnection conn)
        {
            var cmdString = "";
#if !DEBUG
            try
            {
#endif

                cmdString = string.Format(InsertTemplate, QuoteOrNot(TableName), GetNames(), GetParameters());
                Debug.WriteLine(cmdString);
                var cmd = new NpgsqlCommand(cmdString, conn);
                if (InsertTimeout != null)
                    cmd.CommandTimeout = InsertTimeout.Value;
                foreach (var col in AllColumns)
                    col.AddParameters(cmd);
                cmd.ExecuteNonQuery();
#if !DEBUG
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to unnest insert with command '" + cmdString + "'.", ex);
            }
#endif
        }

        public async Task InsertAsync(NpgsqlConnection conn)
        {
            var cmdString = "";
#if !DEBUG
            try
            {
#endif
                cmdString = string.Format(InsertTemplate, QuoteOrNot(TableName), GetNames(), GetParameters());
                Debug.WriteLine(cmdString);
                using (var cmd = new NpgsqlCommand(cmdString, conn))
                {
                    if (InsertTimeout != null)
                        cmd.CommandTimeout = InsertTimeout.Value;
                    foreach (var col in AllColumns)
                        col.AddParameters(cmd);
                    await cmd.ExecuteNonQueryAsync();
                }
#if !DEBUG
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to unnest insert with command '" + cmdString + "'.", ex);
            }
#endif
        }

        private string QuoteOrNot(string item)
        {
            if (QuoteNames) return _cmdBuilder.QuoteIdentifier(item);
            return item;
        }

        public void Add(string key, IEnumerable<int> values)
        {
            UnnestColumns.Add(new UnnestableColumn<int>() { Name = key, Value = values.ToList() });
        }

        public void Add(string key, IEnumerable<long> values)
        {
            UnnestColumns.Add(new UnnestableColumn<long>() { Name = key, Value = values.ToList() });
        }

        public void Add(string key, IEnumerable<double> values)
        {
            UnnestColumns.Add(new UnnestableColumn<double>() { Name = key, Value = values.ToList() });
        }

        public void Add(string key, IEnumerable<string> values)
        {
            UnnestColumns.Add(new UnnestableColumn<string>() { Name = key, Value = values.ToList() });
        }

        public void Add(string key, IEnumerable<bool> values)
        {
            UnnestColumns.Add(new UnnestableColumn<bool>() { Name = key, Value = values.ToList() });
        }

        public void Add(string key, IEnumerable<DateTime> values)
        {
            UnnestColumns.Add(new UnnestableColumn<DateTime>() { Name = key, Value = values.ToList() });
        }

        public void Add(string key, Type type, IEnumerable<object> values)
        {
            UnnestColumns.Add(new AnonymousUnnestableColumn(key, type, values));
        }

        /// <summary>
        /// Inserts points with SRID using ST_SetSrid(ST_MakePoint(UNNEST(@geometryX),UNNEST(@geometryY)), @srid)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="srid"></param>
        public void AddPoint(string key, IEnumerable<double> x, IEnumerable<double> y, int srid)
        {
            UnnestColumns.Add(new CustomUnnestableColumn("geometry", "ST_SetSrid(ST_MakePoint(UNNEST(@geometryX),UNNEST(@geometryY)), " + srid + ")", cmd =>
            {
                cmd.Parameters.AddWithValue($"@geometryX", x.ToArray());
                cmd.Parameters.AddWithValue($"@geometryY", y.ToArray());
            }));
        }

        /// <summary>
        /// Inserts points without SRID using ST_MakePoint(UNNEST(@geometryX),UNNEST(@geometryY))
        /// </summary>
        /// <param name="key"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void AddPoint(string key, IEnumerable<double> x, IEnumerable<double> y)
        {
            UnnestColumns.Add(new CustomUnnestableColumn("geometry", "ST_MakePoint(UNNEST(@geometryX),UNNEST(@geometryY))", cmd =>
            {
                cmd.Parameters.AddWithValue($"@geometryX", x.ToArray());
                cmd.Parameters.AddWithValue($"@geometryY", y.ToArray());
            }));
        }

        /// <summary>
        /// Inserts WKT defined geometries using ST_GeomFromText(unnest(@wkt)).
        /// </summary>
        /// <param name="key"></param>
        /// <param name="wkt"></param>
        public void AddGeometry(string key, IEnumerable<string> wkt)
        {
            UnnestColumns.Add(new CustomUnnestableColumn("geometry", "ST_GeomFromText(unnest(@wkt))", cmd =>
            {
                cmd.Parameters.AddWithValue("@wkt", wkt.ToArray());
            }));
        }

        /// <summary>
        /// Inserts WKT defined geometries using ST_GeomFromText(unnest(@wkt), @srid).
        /// </summary>
        /// <param name="key"></param>
        /// <param name="wkt"></param>
        /// <param name="srid"></param>
        public void AddGeometry(string key, IEnumerable<string> wkt, int srid)
        {
            UnnestColumns.Add(new CustomUnnestableColumn("geometry", "ST_GeomFromText(unnest(@wkt), @srid)", cmd =>
            {
                cmd.Parameters.AddWithValue("@wkt", wkt.ToArray());
                cmd.Parameters.AddWithValue("@srid", srid);
            }));
        }

        /// <summary>
        /// Inserts WKT defined geometries using ST_GeomFromText(unnest(@wkt), unnest(@srid)).
        /// </summary>
        /// <param name="key"></param>
        /// <param name="wkt"></param>
        /// <param name="srid"></param>
        public void AddGeometry(string key, IEnumerable<string> wkt, IEnumerable<int> srid)
        {
            UnnestColumns.Add(new CustomUnnestableColumn("geometry", "ST_GeomFromText(unnest(@wkt), unnest(@srid))", cmd =>
            {
                cmd.Parameters.AddWithValue("@wkt", wkt.ToArray());
                cmd.Parameters.AddWithValue("@srid", srid.ToArray());
            }));
        }

        public void Add(string key, IEnumerable<TimeSpan> values)
        {
            UnnestColumns.Add(new UnnestableColumn<TimeSpan>() { Name = key, Value = values.ToList() });
        }

        public void AddStatic(string key, int value)
        {
            UnnestColumns.Add(new StaticColumn<int>() { Name = key, Value = value });
        }

        public void AddStatic(string key, double value)
        {
            UnnestColumns.Add(new StaticColumn<double>() { Name = key, Value = value });
        }

        public void AddStatic(string key, float value)
        {
            UnnestColumns.Add(new StaticColumn<double>() { Name = key, Value = value });
        }

        public void AddStatic(string key, string value)
        {
            UnnestColumns.Add(new StaticColumn<string>() { Name = key, Value = value });
        }

        public void AddStatic(string key, bool value)
        {
            UnnestColumns.Add(new StaticColumn<bool>() { Name = key, Value = value });
        }

        public void AddStatic(string key, DateTime value)
        {
            UnnestColumns.Add(new StaticColumn<DateTime>() { Name = key, Value = value });
        }

        public void AddStatic(string key, TimeSpan value)
        {
            UnnestColumns.Add(new StaticColumn<TimeSpan>() { Name = key, Value = value });
        }

        private string GetNames()
        {
            return string.Join(", ", AllColumns.Select(p => QuoteOrNot(p.Name)));
        }

        private string GetParameters()
        {
            return string.Join(", ", AllColumns.Select(p => p.GetValueString()));
        }
    }
    
    public abstract class IUnnestableColumn
    {
        public string Name { get; set; }
        public abstract void AddParameters(NpgsqlCommand cmd);

        public virtual string GetValueString()
        {
            return "@" + Name;
        }
    }

    public class UnnestableColumn<T> : IUnnestableColumn
    {
        public List<T> Value { get; set; }
        public override void AddParameters(NpgsqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@" + Name, Value.ToArray());
        }

        public override string GetValueString()
        {
            return "unnest(@" + Name + ")";
        }
    }

    public class CustomUnnestableColumn : IUnnestableColumn
    {
        private readonly string _valueString;
        private readonly Action<NpgsqlCommand> _addParametersAction;

        public CustomUnnestableColumn(string name, string valueString, Action<NpgsqlCommand> addParametersAction = null)
        {
            Name = name;
            _valueString = valueString;
            _addParametersAction = addParametersAction ?? (p => {});
        }

        public override void AddParameters(NpgsqlCommand cmd)
        {
            _addParametersAction(cmd);
        }

        public override string GetValueString()
        {
            return _valueString;
        }
    }

    public class AnonymousUnnestableColumn : IUnnestableColumn
    {
        public object[] Value { get; set; }
        private readonly Type _type;

        public AnonymousUnnestableColumn(string key, Type type, IEnumerable<object> values)
        {
            Name = key;
            _type = type;
            Value = values.ToArray();
        }

        public override void AddParameters(NpgsqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@" + Name, NpgsqlDbType.Array | GetNpgsqlDbType(_type), Value);
        }

        private readonly Dictionary<Type, NpgsqlDbType> _types = new Dictionary<Type, NpgsqlDbType>()
        {
            {typeof(string), NpgsqlDbType.Text},
            {typeof(short), NpgsqlDbType.Smallint},
            {typeof(byte), NpgsqlDbType.Smallint},
            {typeof(long), NpgsqlDbType.Bigint},
            {typeof(int), NpgsqlDbType.Integer},
            {typeof(float), NpgsqlDbType.Real},
            {typeof(double), NpgsqlDbType.Double},
            {typeof(decimal), NpgsqlDbType.Money},
            {typeof(bool), NpgsqlDbType.Boolean},
            {typeof(short?), NpgsqlDbType.Smallint},
            {typeof(byte?), NpgsqlDbType.Smallint},
            {typeof(long?), NpgsqlDbType.Bigint},
            {typeof(int?), NpgsqlDbType.Integer},
            {typeof(float?), NpgsqlDbType.Real},
            {typeof(double?), NpgsqlDbType.Double},
            {typeof(decimal?), NpgsqlDbType.Money},
            {typeof(bool?), NpgsqlDbType.Boolean},
            {typeof(DateTime), NpgsqlDbType.Timestamp},
            {typeof(TimeSpan), NpgsqlDbType.Timestamp}
        };
        private NpgsqlDbType GetNpgsqlDbType(Type type)
        {
            if (_types.TryGetValue(type, out var dbt)) return dbt;
            throw new NotImplementedException("Unnest inserter not implemented for type " + type.Name);
        }

        public override string GetValueString()
        {
            return "unnest(@" + Name + ")";
        }
    }

    public class StaticColumn<T> : IUnnestableColumn
    {
        public T Value { get; set; }
        public override void AddParameters(NpgsqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@" + Name, Value);
        }
    }
}
