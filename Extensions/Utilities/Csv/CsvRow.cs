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
        public string this[string column] => Raw[GetHeaderIndex(column)];

        private int GetHeaderIndex(string column)
        {
            if (_headers.TryGetValue(column, out var index)) return index;
            throw new KeyNotFoundException("The header '" + column + "' does not exist. See the Headers property for available headers.");
        }

        public CsvRow(IEnumerable<string> row, Dictionary<string, int> headers)
        {
            Raw = row.ToArray();
            _headers = headers;
        }

        public bool HasHeader(string header)
        {
            return _headers.ContainsKey(header);
        }

        public bool TryGetString(int ix, out string value)
        {
            value = null;
            if (ix < 0 || ix >= Raw.Length)
                return false;

            value = Raw[ix];
            return true;
        }

        public bool TryGetString(string column, out string value)
        {
            value = null;
            if (!_headers.TryGetValue(column, out var ix))
                return false;

            value = Raw[ix];
            return true;
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

        public static string GetString(this CsvRow row, object indexOrColumn, string defaultValue)
        {
            if (indexOrColumn is string s)
                return row.TryGetString(s, out var v) ? v : defaultValue;

            if (indexOrColumn is int i)
                return row.TryGetString(i, out var v) ? v : defaultValue;

            throw new Exception("Column must be given either as an int index or as a string column name (not '" + indexOrColumn.GetType().Name + "').");
        }

        public static bool ContainsColumn(this CsvRow row, object indexOrColumn)
        {
            return !string.IsNullOrEmpty(GetString(row, indexOrColumn));
        }

        public static int GetInt32(this CsvRow row, object indexOrColumn, int? defaultValue = null)
        {
            var s = GetString(row, indexOrColumn, null);
            if (string.IsNullOrWhiteSpace(s))
            {
                if (defaultValue.HasValue) return defaultValue.Value;
                throw new Exception("Missing column: " + indexOrColumn);
            }
            if (int.TryParse(s, out var v)) return v;
            throw new Exception("The string '" + s + "' could not be parsed as an integer.");
        }

        public static double GetDouble(this CsvRow row, object indexOrColumn, IFormatProvider provider = null, double? defaultValue = null)
        {
            var s = GetString(row, indexOrColumn, null);
            if (string.IsNullOrWhiteSpace(s))
            {
                if (defaultValue.HasValue) return defaultValue.Value;
                throw new Exception("Missing column: " + indexOrColumn);
            }
            if (double.TryParse(s, NumberStyles.Any, provider ?? CultureInfo.InvariantCulture, out var v)) return v;
            throw new Exception("The string '" + s + "' could not be parsed as a decimal number (double).");
        }

        public static DateTime GetDateTime(this CsvRow row, object indexOrColumn, string format, IFormatProvider provider = null, DateTime? defaultValue = null)
        {
            var s = GetString(row, indexOrColumn, null);
            if (string.IsNullOrWhiteSpace(s))
            {
                if (defaultValue.HasValue) return defaultValue.Value;
                throw new Exception("Missing column: " + indexOrColumn);
            }
            if (DateTime.TryParseExact(s, format, provider ?? CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out var v)) return v;
            throw new Exception($"The string '{s}' could not be parsed as a DateTime in the format '{format}'.");
        }

        public static DateTime GetDateTime(this CsvRow row, object indexOrColumn, DateTime? defaultValue = null)
        {
            var s = GetString(row, indexOrColumn, null);
            if (string.IsNullOrWhiteSpace(s))
            {
                if (defaultValue.HasValue) return defaultValue.Value;
                throw new Exception("Missing column: " + indexOrColumn);
            }
            if (DateTime.TryParse(s, out var v)) return v;
            throw new Exception($"The string '{s}' could not be parsed as a DateTime.");
        }

        public static DateTime GetDateTime(this CsvRow row, object indexOrColumn, IFormatProvider provider, DateTime? defaultValue = null)
        {
            var s = GetString(row, indexOrColumn, null);
            if (string.IsNullOrWhiteSpace(s))
            {
                if (defaultValue.HasValue) return defaultValue.Value;
                throw new Exception("Missing column: " + indexOrColumn);
            }
            if (DateTime.TryParse(s, provider, DateTimeStyles.None, out var v)) return v;
            throw new Exception($"The string '{s}' could not be parsed as a DateTime.");
        }
    }
}