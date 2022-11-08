using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Extensions.Utilities.Csv
{
    public class CsvWriter : IDisposable
    {
        private readonly string _separator;
        private readonly string _quote;
        private readonly string _replaceNewLinesWith;
        private readonly string _targetPath;
        private StreamWriter _targetFile;

        public CsvWriter(string targetPath, string separator, string quote = "\"", string replaceNewLinesWith = " ")
        {
            _separator = separator;
            _quote = quote;
            _replaceNewLinesWith = replaceNewLinesWith;
            _targetPath = targetPath;
        }

        public string QuoteValue(string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            if (_replaceNewLinesWith != null)
                value = value.Replace(Environment.NewLine, _replaceNewLinesWith).Replace("\r", _replaceNewLinesWith).Replace("\n", _replaceNewLinesWith);
            value = value.Replace("\\", "\\\\");
            value = value.Replace(_quote, "\\" + _quote);
            if (value.Contains(_separator)) return _quote + value + _quote;
            return value;
        }

        public string QuoteValues(IEnumerable<string> values)
        {
            return string.Join(_separator, values.Select(QuoteValue));
        }

        public void WriteLine(IEnumerable<string> values)
        {
            if (_targetFile == null) _targetFile = new StreamWriter(_targetPath);
            _targetFile.WriteLine(QuoteValues(values));
        }

        public void Dispose()
        {
            _targetFile?.Dispose();
        }
    }
}
