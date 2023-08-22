using System;
using System.Collections.Generic;
using System.Text;

namespace Extensions.Utilities.Csv
{
    public class CsvStringWriter : IDisposable
    {
        private readonly CsvWriter _writer;
        private readonly StringBuilder _sb;

        public string Csv => _sb.ToString();

        public CsvStringWriter(string separator, string quote = "\"", string replaceNewLinesWith = " ")
        {
            _sb = new StringBuilder();
            _writer = new CsvWriter(null, separator, quote, replaceNewLinesWith);
        }

        public void WriteLine(params string[] values)
        {
            _sb.AppendLine(_writer.QuoteValues(values));
        }

        public void WriteLine(params object[] values)
        {
            _sb.AppendLine(_writer.QuoteValues(values));
        }

        public void WriteLine(IEnumerable<string> values)
        {
            _sb.AppendLine(_writer.QuoteValues(values));
        }

        public void WriteLine(IEnumerable<object> values)
        {
            _sb.AppendLine(_writer.QuoteValues(values));
        }

        public void WriteAllLines(IEnumerable<IEnumerable<string>> contents)
        {
            foreach (var line in contents)
                _sb.AppendLine(_writer.QuoteValues(line));
        }

        public void WriteAllLines(IEnumerable<IEnumerable<object>> contents)
        {
            foreach (var line in contents)
                _sb.AppendLine(_writer.QuoteValues(line));
        }

        public void Dispose()
        {
            _writer?.Dispose();
            _sb?.Clear();
        }
    }
}