using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Extensions.Utilities.Csv
{
    public class CsvRow
    {
        private readonly Dictionary<string, int> _headers;

        public string[] Raw { get; set; }
        public string[] Headers => _headers.Keys.ToArray();

        public string this[int index] => Raw[index];
        public string this[string column]
        {
            get
            {
                if (_headers.TryGetValue(column, out var index)) return Raw[index];
                throw new KeyNotFoundException("The header '" + column + "' does not exist. See the Headers property for available headers.");
            }
        }

        public CsvRow(IEnumerable<string> row, Dictionary<string, int> headers)
        {
            Raw = row.ToArray();
            _headers = headers;
        }
    }

    public static class CsvRowExtensions
    {
        public static string GetString(this CsvRow row, object indexOrColumn)
        {
            if (indexOrColumn is string s)
                return row[s];
            if (indexOrColumn is int i)
                return row[i];
            throw new Exception("Column must be given either as an int index or as a string column name (not '" + indexOrColumn.GetType().Name + "').");
        }

        public static int GetInt32(this CsvRow row, object indexOrColumn)
        {
            return int.Parse(GetString(row, indexOrColumn));
        }

        public static double GetDouble(this CsvRow row, object indexOrColumn, IFormatProvider provider = null)
        {
            return double.Parse(GetString(row, indexOrColumn), provider ?? CultureInfo.InvariantCulture);
        }

        public static DateTime GetDateTime(this CsvRow row, object indexOrColumn, string format, IFormatProvider provider = null)
        {
            return DateTime.ParseExact(GetString(row, indexOrColumn), format, provider ?? CultureInfo.InvariantCulture);
        }

        public static DateTime GetDateTime(this CsvRow row, object indexOrColumn)
        {
            return DateTime.Parse(GetString(row, indexOrColumn));
        }

        public static DateTime GetDateTime(this CsvRow row, object indexOrColumn, IFormatProvider provider)
        {
            return DateTime.Parse(GetString(row, indexOrColumn), provider);
        }
    }
}