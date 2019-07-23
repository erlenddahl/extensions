using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extensions;

namespace SqliteExtensions
{
    public abstract class SQLiteEntity
    {
        public long Id { get; set; }

        protected static IEnumerable<Dictionary<string, object>> InternalFromDb(SQLiteConnection conn, string tableName, string where = "", params object[] parameters)
        {
            return InternalFromDbLight(conn, tableName, "*", where, parameters);
        }

        protected static IEnumerable<Dictionary<string, object>> InternalFromDbLight(SQLiteConnection conn, string tableName, string select = "*", string where = "", params object[] parameters)
        {
            if (!string.IsNullOrEmpty(where))
            {
                where = where.TrimStart();
                if (!where.ToUpper().StartsWith("WHERE")) where = "WHERE " + where;
                where = " " + where;
            }
            return ("SELECT " + select + " FROM " + tableName + where).FetchAsync(conn, parameters);
        }

        public static IEnumerable<T> InternalFromDbQuick<T>(SQLiteConnection conn, string tableName, Func<SQLiteDataReader, T> func, string select = "*", string where = "", params object[] parameters)
        {
            return InternalFromDbQuick(conn, tableName, func, select, where, -1, parameters);
        }

        public static IEnumerable<T> InternalFromDbQuick<T>(SQLiteConnection conn, string tableName, Func<SQLiteDataReader, T> func, string select = "*", string where = "", int limit = -1, params object[] parameters)
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
        /// Generate an SQLiteCommand for inserting this object into the database.
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        public abstract SQLiteCommand GetInsertCommand(SQLiteConnection conn);

        /// <summary>
        /// Populate the given command with the entity's parameters.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public abstract SQLiteCommand PopulateParameters(SQLiteCommand cmd);

        /// <summary>
        /// Generate an SQLiteCommand for updating this object in the database.
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        public abstract SQLiteCommand GetUpdateCommand(SQLiteConnection conn);

        public void Insert(SQLiteConnection db, bool updateId = true)
        {
            GetInsertCommand(db).ExecuteNonQuery();
            if (updateId)
                Id = db.LastInsertRowId;
        }

        protected SQLiteCommand GetDeleteCommand(SQLiteConnection conn, string tableName)
        {
            return new SQLiteCommand("DELETE FROM " + tableName + " WHERE id = " + Id, conn);
        }
    }
}
