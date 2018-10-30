using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqliteExtensions
{
    public static class SQLiteEntityExtensions
    {
        /// <summary>
        /// Will insert all entities in the list using a transaction.
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="conn"></param>
        public static int InsertAll(this IEnumerable<SQLiteEntity> entities, SQLiteConnection conn)
        {
            var count = 0;
            SQLiteCommand cmd = null;
            using (var trans = conn.BeginTransaction())
            {
                foreach (var entity in entities)
                {
                    cmd = cmd == null ? entity.GetInsertCommand(conn) : entity.PopulateParameters(cmd);
                    var res = cmd.ExecuteNonQuery();
                    if (res != 1) Debug.WriteLine(res);
                    entity.Id = conn.LastInsertRowId;
                    count++;
                }
                trans.Commit();
            }
            cmd?.Dispose();
            return count;
        }

        /// <summary>
        /// Will save all entities in the list using a transaction. If the item has an Id > 0, it will be updated.
        /// Otherwise, it will be inserted as a new row.
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="conn"></param>
        public static int SaveAll(this IEnumerable<SQLiteEntity> entities, SQLiteConnection conn)
        {
            var count = 0;
            bool prevWasInsert = true;
            SQLiteCommand cmd = null;
            using (var trans = conn.BeginTransaction())
            {
                foreach (var entity in entities)
                {
                    var currIsInsert = entity.Id <= 0;
                    if (!currIsInsert)
                        cmd = entity.GetUpdateCommand(conn);
                    else if (cmd == null || !prevWasInsert)
                        cmd = entity.GetInsertCommand(conn);
                    else
                        entity.PopulateParameters(cmd);

                    prevWasInsert = currIsInsert;
                    var res = cmd.ExecuteNonQuery();

                    if (currIsInsert)
                        entity.Id = conn.LastInsertRowId;

                    if (res != 1) Debug.WriteLine(res);
                    count++;
                }
                trans.Commit();
            }
            if (cmd != null) cmd.Dispose();
            return count;
        }
    }
}
