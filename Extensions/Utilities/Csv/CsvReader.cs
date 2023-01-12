using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions.Utilities.Csv
{
    public class CsvReader
    {
        private char _separator;
        private readonly char _quote;
        private readonly bool _hasHeaders;
        private readonly bool _lowercaseHeaders;

        public CsvReader(char separator = ';', char quote = '"', bool hasHeaders = true, bool lowercaseHeaders = false)
        {
            _separator = separator;
            _quote = quote;
            _hasHeaders = hasHeaders;
            _lowercaseHeaders = lowercaseHeaders;
        }

        public IEnumerable<string> SplitRow(string row)
        {
            var currStart = 0;
            var insideQuotes = false;
            var isEscaped = false;
            for (var i = 0; i < row.Length; i++)
            {
                if (CheckEscape(row[i], ref isEscaped)) continue;
                if (IsNewColumn(row[i], ref insideQuotes))
                {
                    yield return row.Substring(currStart, i - currStart).Trim(_quote);
                    currStart = i + 1;
                }
            }

            if (currStart <= row.Length)
                yield return row.Substring(currStart, row.Length - currStart).Trim(_quote);
        }

        private bool CheckEscape(char c, ref bool isEscaped)
        {
            if (!isEscaped && c == '\\')
            {
                isEscaped = true;
                return true;
            }

            if (isEscaped)
            {
                isEscaped = false;
                return true;
            }

            return false;
        }

        private bool IsNewColumn(char c, ref bool insideQuotes)
        {
            if (c == _quote)
                insideQuotes = !insideQuotes;
            else if (!insideQuotes && c == _separator)
                return true;

            return false;
        }

        public string SplitRowAndRetrieveSingleColumn(string row, int column)
        {
            var currStart = 0;
            var insideQuotes = false;
            var isEscaped = false;
            int currentColumn = 0;
            for (var i = 0; i < row.Length; i++)
            {
                if (CheckEscape(row[i], ref isEscaped)) continue;
                if (IsNewColumn(row[i], ref insideQuotes))
                {
                    if (currentColumn == column) return row.Substring(currStart, i - currStart).Trim(_quote);
                    currentColumn++;
                    currStart = i + 1;
                }
            }

            if (currStart <= row.Length && currentColumn == column)
                return row.Substring(currStart, row.Length - currStart).Trim(_quote);

            throw new Exception($"Column {column:n0} not found.");
        }

        public IEnumerable<CsvRow> ReadFile(string filename)
        {
            var headers = new Dictionary<string, int>();
            if (_hasHeaders)
                headers = ParseHeaders(System.IO.File.ReadLines(filename).First());
            return System.IO.File.ReadLines(filename).Skip(_hasHeaders ? 1 : 0).Select(SplitRow).Select(p => new CsvRow(p, headers));
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
            return SplitRow(headerRow).Select((a, b) => new {Header = a, Index = b}).ToDictionary(k => _lowercaseHeaders ? k.Header.ToLower() : k.Header, v => v.Index);
        }
    }
}