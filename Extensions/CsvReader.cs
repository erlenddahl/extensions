using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public class CsvWriter
    {
        private readonly string _separator;

        public CsvWriter(string separator)
        {
            _separator = separator;
        }

        public string QuoteValue(string value)
        {
            if (value.Contains(_separator)) return "\"" + value + "\"";
            return value;
        }
    }

    public class CsvReader
    {
        private char _separator;
        private readonly char _quote;
        private readonly bool _hasHeaders;

        public CsvReader(char separator = ';', char quote = '"', bool hasHeaders = true)
        {
            _separator = separator;
            _quote = quote;
            _hasHeaders = hasHeaders;
        }

        private IEnumerable<string> SplitRow(string row)
        {
            var currStart = 0;
            var insideQuotes = false;
            for (var i = 0; i < row.Length; i++)
            {
                if (row[i] == _quote)
                {
                    insideQuotes = !insideQuotes;
                }
                else if (!insideQuotes && row[i] == _separator)
                {
                    yield return row.Substring(currStart, i - currStart).Trim(_quote).Replace("\"\"", "\"");
                    currStart = i + 1;
                }
            }

            if(currStart < row.Length)
                yield return row.Substring(currStart, row.Length - currStart).Trim(_quote).Replace("\"\"", "\"");
        }

        public IEnumerable<CsvRow> ReadFile(string filename)
        {
            var headers = new Dictionary<string, int>();
            if (_hasHeaders)
                headers = ParseHeaders(System.IO.File.ReadLines(filename).First());
            return System.IO.File.ReadLines(filename).Skip(_hasHeaders ? 1 : 0).Select(SplitRow).Select(p => new CsvRow(p, headers));
        }

        public IEnumerable<CsvRow> ReadString(string filename)
        {
            return ReadLines(filename.Split(Environment.NewLine));
        }

        public IEnumerable<CsvRow> ReadLines(string[] lines)
        {
            var headers = new Dictionary<string, int>();
            if (_hasHeaders)
                headers = ParseHeaders(lines.First());
            return lines.Skip(_hasHeaders ? 1 : 0).Select(SplitRow).Where(p => p.Any()).Select(p => new CsvRow(p, headers));
        }

        private Dictionary<string, int> ParseHeaders(string headerRow)
        {
            return SplitRow(headerRow).Select((a, b) => new { Header = a, Index = b }).ToDictionary(k => k.Header, v => v.Index);
        }
    }

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
}
