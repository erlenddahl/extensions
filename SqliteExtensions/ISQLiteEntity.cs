using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extensions;

namespace SqliteExtensions
{
    public abstract class SQLiteEntity
    {
        public long Id { get; set; }

        protected static IEnumerable<Dictionary<string, object>> InternalFromDb(SqliteConnection conn, string tableName, string where = "", params object[] parameters)
        {
            return InternalFromDbLight(conn, tableName, "*", where, parameters);
        }

        protected static IEnumerable<Dictionary<string, object>> InternalFromDbLight(SqliteConnection conn, string tableName, string select = "*", string where = "", params object[] parameters)
        {
            if (!string.IsNullOrEmpty(where))
            {
                where = where.TrimStart();
                if (!where.ToUpper().StartsWith("WHERE")) where = "WHERE " + where;
                where = " " + where;
            }
            return ("SELECT " + select + " FROM " + tableName + where).FetchAsync(conn, parameters);
        }

        public static IEnumerable<T> InternalFromDbQuick<T>(SqliteConnection conn, string tableName, Func<SqliteDataReader, T> func, string select = "*", string where = "", params object[] parameters)
        {
            return InternalFromDbQuick(conn, tableName, func, select, where, -1, parameters);
        }

        public static IEnumerable<T> InternalFromDbQuick<T>(SqliteConnection conn, string tableName, Func<SqliteDataReader, T> func, string select = "*", string where = "", int limit = -1, params object[] parameters)
        {
            if (!string.IsNullOrEmpty(where))
            {
                where = where.TrimStart();
                if (!where.ToUpper().StartsWith("WHERE")) where = "WHERE " + where;
                where = " " + where;
            }
            var limitPart = limit < 0 ? "" : " LIMIT " + limit;
            return ("SELECT " + select + " FROM " + tableName + where + limitPart).FetchAsync(conn, func, parameters);
        }

        /// <summary>
        /// Generate an SqliteCommand for inserting this object into the database.
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        public abstract SqliteCommand GetInsertCommand(SqliteConnection conn);

        /// <summary>
        /// Populate the given command with the entity's parameters.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public abstract SqliteCommand PopulateParameters(SqliteCommand cmd);

        /// <summary>
        /// Generate an SqliteCommand for updating this object in the database.
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        public abstract SqliteCommand GetUpdateCommand(SqliteConnection conn);

        public void Insert(SqliteConnection db, bool updateId = true)
        {
            GetInsertCommand(db).ExecuteNonQuery();
            if (updateId)
                Id = GetLastInsertedRowId(db);
        }

        private static long GetLastInsertedRowId(SqliteConnection conn)
        {
            return (long)new SqliteCommand("SELECT last_insert_rowid();", conn).ExecuteScalar();
        }

        protected SqliteCommand GetDeleteCommand(SqliteConnection conn, string tableName)
        {
            return new SqliteCommand("DELETE FROM " + tableName + " WHERE id = " + Id, conn);
        }
    }
}
