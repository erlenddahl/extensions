using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqliteExtensions
{
    public static class GetNullable
    {
        public static int? GetNullableInt32(this SQLiteDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;
            return reader.GetInt32(index);
        }

        public static long? GetNullableInt64(this SQLiteDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;
            return reader.GetInt64(index);
        }

        public static string GetNullableString(this SQLiteDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;
            return reader.GetString(index);
        }

        public static bool? GetNullableBoolean(this SQLiteDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;
            return reader.GetBoolean(index);
        }

        public static DateTime? GetNullableDateTime(this SQLiteDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;
            return reader.GetDateTime(index);
        }

        public static double? GetNullableDouble(this SQLiteDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;
            return reader.GetDouble(index);
        }
    }
}
