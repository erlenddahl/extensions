using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqliteExtensions
{
    public static class GetNullable
    {
        public static int? GetNullableInt32(this SqliteDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;
            return reader.GetInt32(index);
        }

        public static long? GetNullableInt64(this SqliteDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;
            return reader.GetInt64(index);
        }

        public static string GetNullableString(this SqliteDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;
            return reader.GetString(index);
        }

        public static bool? GetNullableBoolean(this SqliteDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;
            return reader.GetBoolean(index);
        }

        public static DateTime? GetNullableDateTime(this SqliteDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;
            return reader.GetDateTime(index);
        }

        public static double? GetNullableDouble(this SqliteDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;
            return reader.GetDouble(index);
        }
    }
}
