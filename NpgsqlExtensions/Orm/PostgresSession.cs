using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using Extensions;
using Extensions.IEnumerable;
using Extensions.Reflection;
using Npgsql;

namespace NpgsqlExtensions.Orm
{
    public class PostgresOrmSession : IDisposable
    {
        private readonly NpgsqlConnection _conn;
        private readonly string _username;

        public PostgresOrmSession(string connString, string username)
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
                for (var i = 0; i < ormType.Columns.Length; i++)
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

        public void Insert<T>(T element, string tableName = null)
        {
            tableName = GetTableName<T>(tableName);
            var ormType = GetOrmType<T>();

            ormType.InsertAndUpdateId(tableName, element, _conn);
        }

        /// <summary>
        /// Inserts all the given elements using an UnnestInserter on subsets of the given subset size.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elements"></param>
        /// <param name="subsetSize"></param>
        /// <param name="tableName"></param>
        public void InsertRange<T>(IList<T> elements, int subsetSize = 1000, string tableName = null)
        {
            tableName = GetTableName<T>(tableName);
            var ormType = GetOrmType<T>();

            foreach (var subset in elements.Sublists(subsetSize))
            {
                var inserter = new UnnestInserter.UnnestInserter(tableName);

                foreach (var col in ormType.Columns.Where(p => !p.IsIdColumn))
                    inserter.Add(col.Name, col.PropertyType, subset.Select(p => col.GetValue(p)));

                inserter.Insert(_conn);
            }
        }

        public void Dispose()
        {
            _conn?.Close();
            _conn?.Dispose();
        }

        public int ExecuteNonQuery(string cmd, params object[] parameters)
        {
            return new NpgsqlCommand(cmd, _conn).SetParameters(parameters).ExecuteNonQuery();
        }

        public object ExecuteScalar(string cmd, params object[] parameters)
        {
            return new NpgsqlCommand(cmd, _conn).SetParameters(parameters).ExecuteScalar();
        }
    }
}