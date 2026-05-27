using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Diagnostics;
using System.Linq;
using net.erlenddahl.Extensions;
using net.erlenddahl.Extensions.StringExtensions;

namespace SqliteExtensions
{
    /// <summary>
    /// This is an extension class to make database queries easier to code.
    /// </summary>
    public static class DbExtensions
    {
        private static bool _debug = false;

        public static long GetLastInsertedRowId(this SqliteConnection conn)
        {
            return (long)new SqliteCommand("SELECT last_insert_rowid();", conn).ExecuteScalar();
        }

        /// <summary>
        /// Returns the number of items in the given table.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="table"></param>
        /// <param name="where"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static long Count(this SqliteConnection conn, string table, string where = "", params object[] parameters)
        {
            var cmd = new SqliteCommand("SELECT COUNT(*) FROM " + table + (string.IsNullOrEmpty(where) ? "" : " WHERE " + where), conn);
            if (parameters != null && parameters.Any())
                cmd.PopulateParameters(parameters);
            return (long)cmd.ExecuteScalar();
        }

        /// <summary>
        /// Deletes all matching rows in the given table.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="table"></param>
        /// <param name="where"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static long EmptyTable(this SqliteConnection conn, string table, string where = "", params object[] parameters)
        {
            var cmd = new SqliteCommand("DELETE FROM " + table + (string.IsNullOrEmpty(where) ? "" : " WHERE " + where), conn);
            if (parameters != null && parameters.Any())
                cmd.PopulateParameters(parameters);
            return (long)cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Executes a database query, and returns a list of dictionaries, where each dctionary represents a row in the result.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static IEnumerable<Dictionary<string, object>> Fetch(this SqliteDataReader reader)
        {
            while (reader.Read())
            {
                var dict = new Dictionary<string, object>();
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    var key = reader.GetName(i);
                    if (reader.GetValue(i) != DBNull.Value)
                        dict.Add(key, reader.GetValue(i));
                    else
                        dict.Add(key, null);
                }
                yield return dict;
            }
            if (!reader.IsClosed)
                reader.Close();
        }

        /// <summary>
        /// Executes a database query, and returns a list of dictionaries, where each dctionary represents a row in the result.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IEnumerable<T> Fetch<T>(this SqliteDataReader reader, Func<SqliteDataReader, T> func)
        {
            while (reader.Read())
            {
#if DEBUG
                T res = default(T);
                try
                {
                    res = func(reader);
                }
                catch (Exception ex)
                {
                    reader.PrintDebug();
                    throw ex;
                }

                yield return res;
#else
                yield return func(reader);
#endif
            }
            if (!reader.IsClosed)
                reader.Close();
        }

        public static void PrintDebug(this SqliteDataReader reader)
        {
            var vals = new List<string>();
            for (var i = 0; i < reader.FieldCount; i++)
                vals.Add(reader.Info(i));
            Debug.WriteLine(string.Join(Environment.NewLine, vals));
        }

        public static string Info(this SqliteDataReader reader, int index)
        {
            var isNull = reader.IsDBNull(index);
            return $"[{index}] {reader.GetName(index)}: {(isNull ? "[NULL]" : reader.GetValue(index))} ({(isNull ? "[NULL]" : reader.GetValue(index).GetType().ToString())}, {reader.GetDataTypeName(index)})";
        }

        public static void PrintSQLiteDebug(this string command, SqliteConnection conn, params object[] parameters)
        {
            var cmd = new SqliteCommand(command, conn);

            if (parameters != null && parameters.Any())
                cmd.PopulateParameters(parameters);

            using (var reader = cmd.ExecuteReader())
            {
                var row = 0;
                while (reader.Read())
                {
                    Debug.WriteLine("Row " + row++);
                    reader.PrintDebug();
                    Debug.WriteLine("");
                }
            }
        }

        /// <summary>
        /// Executes a database query, and returns a single value.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="conn"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T FetchSingle<T>(this string command, SqliteConnection conn, params object[] parameters)
        {
            return new SqliteCommand(command, conn).FetchSingle<T>(parameters);
        }

        /// <summary>
        /// Executes a database query, and returns a list of dictionaries, where each dctionary represents a row in the result.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T FetchSingle<T>(this SqliteCommand cmd, params object[] parameters)
        {
            return (T)cmd.Fetch(parameters).First().Values.First();
        }

        /// <summary>
        /// Executes a database query, and returns a single value
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static T FetchSingle<T>(this SqliteDataReader reader)
        {
            return (T)reader.Fetch().First().Values.First();
        }

        /// <summary>
        /// Executes a database query, and returns a list of dictionaries, where each dctionary represents a row in the result.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> Fetch(this SqliteCommand cmd, params object[] parameters)
        {
            if (parameters != null && parameters.Any())
                cmd.PopulateParameters(parameters);

            using (var reader = cmd.ExecuteReader())
            {
                return reader.Fetch().ToList();
            }
        }

        /// <summary>
        /// Executes a database query, and returns an ienumerable of dictionaries, where each dctionary represents a row in the result.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static IEnumerable<Dictionary<string, object>> FetchAsync(this SqliteCommand cmd, params object[] parameters)
        {
            if (parameters != null && parameters.Any())
                cmd.PopulateParameters(parameters);

            var reader = cmd.ExecuteReader();
            return reader.Fetch();
        }

        /// <summary>
        /// Executes a database query, and returns an ienumerable of dictionaries, where each dctionary represents a row in the result.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="func"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static IEnumerable<T> FetchAsync<T>(this SqliteCommand cmd, Func<SqliteDataReader, T> func, params object[] parameters)
        {
            if (parameters != null && parameters.Any())
                cmd.PopulateParameters(parameters);

            var reader = cmd.ExecuteReader();
            return reader.Fetch(func);
        }

        /// <summary>
        /// Executes a database query, and returns a list of dictionaries, where each dictionary represents a row in the result.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="conn"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> Fetch(this string command, SqliteConnection conn, params object[] parameters)
        {
            return new SqliteCommand(command, conn).Fetch(parameters);
        }

        /// <summary>
        /// Executes a database query, and returns a list of dictionaries, where each dictionary represents a row in the result.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="conn"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static IEnumerable<Dictionary<string, object>> FetchAsync(this string command, SqliteConnection conn, params object[] parameters)
        {
            return new SqliteCommand(command, conn).FetchAsync(parameters);
        }

        /// <summary>
        /// Executes a database query, and returns a list of dictionaries, where each dictionary represents a row in the result.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="conn"></param>
        /// <param name="func"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static IEnumerable<T> FetchAsync<T>(this string command, SqliteConnection conn, Func<SqliteDataReader, T> func, params object[] parameters)
        {
            return new SqliteCommand(command, conn).FetchAsync(func, parameters);
        }

        /// <summary>
        /// Executes a non-query, and returns the number of rows affected.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(this SqliteCommand cmd, params object[] parameters)
        {
            cmd.PopulateParameters(parameters);

            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Executes a non-query, and returns the number of rows affected.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="conn"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(this string command, SqliteConnection conn, params object[] parameters)
        {
            return new SqliteCommand(command, conn).ExecuteNonQuery(parameters);
        }

        /// <summary>
        /// Goes through a list of parameter values, and creates OleDbParameters using @i as they key,
        /// and the parameter at position i as the value. 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parameters"></param>
        public static SqliteCommand PopulateParameters(this SqliteCommand cmd, params object[] parameters)
        {
            var parId = 0;
            cmd.Parameters.Clear();
            foreach (var par in parameters)
            {
                var para = new SqliteParameter("@" + parId++, GetDbType(par))
                {
                    Value = par ?? DBNull.Value
                };
                cmd.Parameters.Add(para);
            }
            return cmd;
        }

        /// <summary>
        /// Returns the OldDbType from an object.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private static DbType GetDbType(object o)
        {
            if (o is byte[]) return DbType.Binary;
            if (o is bool) return DbType.Boolean;
            if (o is int) return DbType.Int32;
            if (o is short) return DbType.Int16;
            if (o is Single || o is long) return DbType.Int64;
            if (o is Double) return DbType.Double;
            if (o is DateTime) return DbType.DateTime;
            if (o is string) return DbType.String;
            if (o == null) return DbType.Object;

            throw new NotImplementedException("GetDbType: " + (o == null ? "null" : o.GetType().ToString()));
        }

        /// <summary>
        /// Goes through a list of parameter values, and creates OleDbParameters using @item1 as they key,
        /// and item2 as the value. 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parameters"></param>
        public static void AddParameters(this SqliteCommand cmd, IEnumerable<Tuple<string, object>> parameters)
        {
            foreach (var p in parameters)
                cmd.Parameters.AddWithValue("@" + p.Item1.CleanAlphaNumeric(), p.Item2 ?? DBNull.Value);
        }

        /// <summary>
        /// Executes a non-query, and returns the number of rows affected.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="conn"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int Execute(this string command, SqliteConnection conn, IEnumerable<Tuple<string, object>> parameters = null)
        {
            var cmd = new SqliteCommand(command, conn);
            if (parameters != null) cmd.AddParameters(parameters);

            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Runs the "VACUUM" command, compacting the database file.
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        public static int Vacuum(this SqliteConnection conn)
        {
            return "vacuum;".Execute(conn);
        }

        /// <summary>
        /// Creates an index on the given table and columns, using an auto generated name (based on the table and columns).
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="table"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static int CreateIndex(this SqliteConnection conn, string table, params string[] columns)
        {
            return conn.CreateNamedIndex(table, table + "_" + string.Join("_", columns), columns);
        }

        /// <summary>
        /// Creates an index on the given table and columns, using the given name.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="table"></param>
        /// <param name="indexName"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static int CreateNamedIndex(this SqliteConnection conn, string table, string indexName, params string[] columns)
        {
            try
            {
                return ("CREATE INDEX " + indexName + " ON " + table + "(" + string.Join(", ", columns) + " ASC)").ExecuteNonQuery(conn);
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("already exists")) throw ex;
                return 0;
            }
        }

        /// <summary>
        /// Returns the number of non-system tables in the database.
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        public static long CountTables(this SqliteConnection conn)
        {
            return "SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name != 'android_metadata' AND name != 'sqlite_sequence';".FetchSingle<long>(conn);
        }

        public static string GetStringSafe(this SqliteDataReader reader, int index)
        {
            return reader.IsDBNull(index) ? null : reader.GetString(index);
        }
    }
}
