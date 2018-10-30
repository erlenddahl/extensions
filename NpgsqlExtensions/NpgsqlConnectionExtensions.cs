using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace NpgsqlExtensions
{
    public static class NpgsqlConnectionExtensions
    {
        public static long? Count(this NpgsqlConnection conn, string query)
        {
            return (long?)new NpgsqlCommand(query, conn).ExecuteScalar();
        }
        public static long? Delete(this NpgsqlConnection conn, string tableName, string query)
        {
            return (long?)new NpgsqlCommand($"DELETE FROM {tableName} WHERE {query}", conn).ExecuteScalar();
        }
    }
}
