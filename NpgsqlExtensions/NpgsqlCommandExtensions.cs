using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using net.erlenddahl.Extensions.IEnumerableExtensions;
using Npgsql;

namespace net.erlenddahl.NpgsqlExtensions
{
    public static class NpgsqlCommandExtensions
    {
        public static IEnumerable<T> ExecuteReaderAndSelect<T>(this NpgsqlCommand cmd, Func<NpgsqlDataReader, T> func)
        {
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                    yield return func(reader);
            }
        }

        public static async Task<Dictionary<TKey, TValue>> ExecuteReaderToDictionary<TKey, TValue>(this NpgsqlCommand cmd, Func<NpgsqlDataReader, TKey> keyFunc, Func<NpgsqlDataReader, TValue> valueFunc)
        {
            var dict = new Dictionary<TKey, TValue>();
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (reader.Read())
                    dict.Add(keyFunc(reader), valueFunc(reader));
            }

            return dict;
        }

        public static void ExecuteReaderForEach(this NpgsqlCommand cmd, Action<NpgsqlDataReader> action)
        {
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                    action(reader);
            }
        }

        public static NpgsqlCommand SetParameters(this NpgsqlCommand cmd, params object[] parameters)
        {
            var pars = parameters.Segment(2, p => new {Key = (string) p[0], Value = p[1]});
            foreach (var par in pars)
                cmd.Parameters.AddWithValue(par.Key, par.Value);
            return cmd;
        }
    }
}
