using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using Extensions;
using Extensions.IEnumerableExtensions;
using Extensions.Reflection;
using Extensions.StringExtensions;
using Extensions.Utilities.Csv;
using Npgsql;
using NpgsqlExtensions.UnnestInserter;

namespace NpgsqlExtensions.Orm
{
    public class PostgresOrmSession : IDisposable
    {
        private readonly NpgsqlConnection _conn;
        private readonly string _username;

        public NpgsqlConnection Connection => _conn;

        public PostgresOrmSession(string connString, string username, int commandTimeout = 60)
        {
            _username = username;
            _conn = OpenConnection(connString);
        }

        private NpgsqlConnection OpenConnection(string connString)
        {
            var tries = 10;
            while (tries >= 0)
            {
                try
                {
                    var conn = new NpgsqlConnection(connString);
                    conn.Open();
                    return conn;
                }
                catch (Exception ex)
                {
                    if (--tries >= 0)
                    {
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            throw new Exception();
        }

        /// <summary>
        /// Retrieves elements matching the given query.
        /// Note that the query will be prefixed with the ColumnString in order to extract all properties.
        /// </summary>
        /// <param name="query">The part after "SELECT * FROM table"</param>
        /// <param name="tableName"></param>
        /// <param name="parameters">Parameters for a prepared statement, if any. Must be supplied in pairs: "name1", value1, "name2", value2, etc.</param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string query = "", string tableName = null, params object[] parameters) where T : new()
        {
            tableName = GetTableName<T>(tableName);
            var ormType = GetOrmType<T>();
            var cmd = ormType.GetSelectQuery(tableName, _conn);

            if (!query.StartsWith(" ")) query = " " + query;
            cmd.CommandText += query;
            cmd.SetParameters(parameters);

            return cmd.ExecuteReaderAndSelect(p =>
            {
                var t = new T();
                for (var i = 0; i < ormType.Columns.Count; i++)
                {
                    if (!ormType.Columns[i].CanSet) continue;
                    var v = p.GetValue(i);
                    if (v is System.DBNull) v = null;
                    ormType.Columns[i].SetValue(t, v);
                }

                return t;
            });
        }

        /// <summary>
        /// Retrieves elements matching the given query.
        /// Note that the query will be prefixed with the ColumnString in order to extract all properties.
        /// </summary>
        /// <param name="query">The complete query"</param>
        /// <param name="func">The function for creating a result items from the current reader row</param>
        /// <param name="parameters">Parameters for a prepared statement, if any. Must be supplied in pairs: "name1", value1, "name2", value2, etc.</param>
        /// <returns></returns>
        public IEnumerable<T> QueryCustom<T>(string query, Func<NpgsqlDataReader, T> func, params object[] parameters)
        {
            var cmd = new NpgsqlCommand(query, _conn);
            cmd.SetParameters(parameters);

            return cmd.ExecuteReaderAndSelect(func);
        }

        /// <summary>
        /// Retrieves elements matching the given query.
        /// Note that the query will be prefixed with the ColumnString in order to extract all properties.
        /// </summary>
        /// <param name="query">The part after "SELECT * FROM table"</param>
        /// <returns></returns>
        public T QuerySingle<T>(string query = "") where T : new()
        {
            if (!query.ToLower().Contains("limit 1")) query += " LIMIT 1";
            return Query<T>(query).FirstOrDefault();
        }

        /// <summary>
        /// Returns the last row ordered by the selected property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyFunc"></param>
        /// <returns></returns>
        public T LastBy<T>(Expression<Func<T, object>> propertyFunc) where T : new()
        {
            var table = GetOrmType<T>();
            return QuerySingle<T>("ORDER BY " + table.Quote(propertyFunc.GetMemberName().ToCamelCase()) + " DESC LIMIT 1");
        }

        /// <summary>
        /// Returns the first row ordered by the selected property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyFunc"></param>
        /// <returns></returns>
        public T FirstBy<T>(Expression<Func<T, object>> propertyFunc) where T : new()
        {
            var table = GetOrmType<T>();
            return QuerySingle<T>("ORDER BY " + table.Quote(propertyFunc.GetMemberName()) + " DESC LIMIT 1");
        }

        public static string GetTableName<T>(string tableName = null)
        {
            if (string.IsNullOrEmpty(tableName)) tableName = typeof(T).Name;
            return tableName.ToLower();
        }

        public void CreateTable<T>(string tableName = null, string createIdColumn = null, bool dropIfExists = false)
        {
            tableName = GetTableName<T>(tableName);
            var ormType = GetOrmType<T>();

            try
            {
                ormType.GetCreationQuery(tableName, _username, createIdColumn, _conn, dropIfExists).ExecuteNonQuery();
            }
            catch (PostgresException pex)
            {
                if (pex.SqlState != "42P07") throw;
            }
        }

        private static readonly Dictionary<Type, PostgresOrmTable> _typeCache = new Dictionary<Type, PostgresOrmTable>();
        public PostgresOrmTable GetOrmType<T>()
        {
            var t = typeof(T);
            if (_typeCache.TryGetValue(t, out var ormType)) return ormType;
            ormType = new PostgresOrmTable(t);
            _typeCache.Add(t, ormType);
            return ormType;
        }

        public void Insert<T>(T element, string tableName = null, PostgresOrmTable ormType = null)
        {
            tableName = GetTableName<T>(tableName);
            ormType = ormType ?? GetOrmType<T>();

            ormType.InsertAndUpdateId(tableName, element, _conn);
        }

        /// <summary>
        /// Inserts all the given elements using an UnnestInserter on subsets of the given subset size.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elements"></param>
        /// <param name="subsetSize"></param>
        /// <param name="tableName"></param>
        public void InsertRange<T>(IList<T> elements, int subsetSize = 1000, string tableName = null, PostgresOrmTable ormType = null)
        {
            tableName = GetTableName<T>(tableName);
            ormType = ormType ?? GetOrmType<T>();

            foreach (var subset in elements.Sublists(subsetSize))
            {
                var inserter = new UnnestInserter.UnnestInserter(tableName);

                foreach (var col in ormType.Columns.Where(p => !p.IsIdColumn))
                {
                    if (col.StaticValue != null)
                        inserter.UnnestColumns.Add(new CustomUnnestableColumn(col.Name, col.StaticValue));
                    else
                        inserter.Add(col.Name, col.PropertyType, subset.Select(p => col.GetValue(p)));
                }

                inserter.Insert(_conn);
            }
        }

        public void Dispose()
        {
            _conn?.Close();
            _conn?.Dispose();
        }

        public NpgsqlTransaction BeginTransaction()
        {
            return _conn.BeginTransaction();
        }

        public int ExecuteNonQuery(string cmd, params object[] parameters)
        {
            return new NpgsqlCommand(cmd, _conn).SetParameters(parameters).ExecuteNonQuery();
        }

        public object ExecuteScalar(string cmd, params object[] parameters)
        {
            return new NpgsqlCommand(cmd, _conn).SetParameters(parameters).ExecuteScalar();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="query">The part after "SELECT * FROM table"</param>
        /// <param name="csvPath"></param>
        /// <param name="separator"></param>
        /// <param name="ignoreColumns">The name of any columns that should be ignored.</param>
        /// <param name="parameters">Parameters for a prepared statement, if any. Must be supplied in pairs: "name1", value1, "name2", value2, etc.</param>
        public void DumpTableAsCsv(string tableName, string csvPath, string separator = ";", string query = "", string[] ignoreColumns = null, params object[] parameters)
        {
            var b = new NpgsqlCommandBuilder();
            var cmd = new NpgsqlCommand("SELECT * FROM public." + b.QuoteIdentifier(tableName), _conn);

            if (!query.StartsWith(" ")) query = " " + query;
            cmd.CommandTimeout = 0;
            cmd.CommandText += query;
            cmd.SetParameters(parameters);
            cmd.AllResultTypesAreUnknown = true;

            string SerializeToString(object value)
            {
                return value?.ToString() ?? "";
            }

            using (var csv= new CsvWriter(csvPath, separator))
            {
                string[] columns = null;
                bool[] useColumn = null;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (columns == null)
                        {
                            columns = reader.GetColumnSchema().Select(p => p.ColumnName).ToArray();
                            useColumn = columns.Select((p, i) => ignoreColumns?.Any() != true|| !ignoreColumns.Contains(p)).ToArray();
                            csv.WriteLine(columns.Where((p, i) => useColumn[i]));
                        }

                        csv.WriteLine(Enumerable.Range(0, reader.FieldCount).Where((p, i) => useColumn[i]).Select(p => SerializeToString(reader.GetValue(p))));
                    }
                }
            }
        }

        /// <summary>
        /// Reads the given CSV file and inserts it to the given table (empties the table first if emptyTableFirst = true).
        /// All rows are by default assumed to be of type TEXT. If another type should be used, defined it using the columnData argument.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="csvPath"></param>
        /// <param name="separator"></param>
        /// <param name="columns"></param>
        /// <param name="dropIfExists"></param>
        /// <param name="subsetSize"></param>
        /// <param name="progressAction"></param>
        public void ImportCsv(string tableName, string csvPath, char separator = ';', IList<PostgresOrmCsvColumn> columns = null, bool dropIfExists = true, int subsetSize = 1000, Action<int> progressAction = null)
        {
            try
            {
                PostgresOrmTable.GetCreationQuery(tableName, columns, p => p, conn: _conn, dropIfExists: dropIfExists).ExecuteNonQuery();
            }
            catch (PostgresException pex)
            {
                if (pex.SqlState != "42P07") throw;
            }

            var csv = new CsvReader(separator);

            var count = 0;
            foreach (var subset in csv.ReadFile(csvPath).Sublists(subsetSize))
            {
                var inserter = new UnnestInserter.UnnestInserter(tableName);

                foreach (var col in columns)
                    col.AddToInserter(inserter, subset);

                inserter.Insert(_conn);
                count += subset.Count;

                progressAction?.Invoke(count);
            }
        }

        /// <summary>
        /// Generates C# code for creating a PostgresOrmColumnCollection with columns from the given table for the ImportCsv function.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string GeneratePostgresOrmColumnCollectionCodeFromDatabaseTable(string tableName)
        {
            var b = new NpgsqlCommandBuilder();
            var cmd = new NpgsqlCommand($"SELECT * FROM public.{b.QuoteIdentifier(tableName)} LIMIT 1", _conn);
            cmd.CommandTimeout = 0;
            var sb = new StringBuilder();
            using (var r = cmd.ExecuteReader())
            {
                foreach (var g in r.GetColumnSchema().Select(p => new {Name = p.ColumnName, Type = p.DataTypeName}).GroupBy(p => PostgresOrmColumn.GetCTypeName(p.Type)))
                {
                    sb.AppendLine("cols." + g.Key.CapitalizeFirst() + "(" + string.Join(", ", g.Select(p => "\"" + p.Name + "\"")) + ");");
                }
            }

            return sb.ToString();
        }

        public void DuplicateTable(string fromTable, string toTable, bool structureOnly = false)
        {
            new NpgsqlCommand($"CREATE TABLE public.{toTable} AS (SELECT * FROM public.{fromTable}){(structureOnly ? " with no data" : "")};", _conn).ExecuteNonQuery();
        }

        public IEnumerable<string> CleanColumnNames(string tableName, bool removeWhitespace = true, bool toLowerCase = true)
        {
            var b = new NpgsqlCommandBuilder();
            var cmd = new NpgsqlCommand($"SELECT * FROM public.{b.QuoteIdentifier(tableName)} LIMIT 1", _conn);
            cmd.CommandTimeout = 0;
            string[] names;
            using (var r = cmd.ExecuteReader())
            {
                names = r.GetColumnSchema().Select(p => p.ColumnName).ToArray();
            }

            foreach (var col in names)
            {
                var newCol = col;
                if (removeWhitespace) newCol = newCol.Trim();
                if (toLowerCase) newCol = newCol.ToLower();

                if (newCol == col)
                    yield return "No change: \"" + col + "\"";
                else
                {
                    new NpgsqlCommand($"ALTER TABLE public.{tableName} RENAME COLUMN \"{col}\" TO \"{newCol}\";", _conn).ExecuteNonQuery();
                    yield return "\"" + col + "\" => \"" + newCol + "\"";
                }
            }
        }
    }
}