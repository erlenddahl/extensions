using System;
using System.Collections.Generic;
using System.Globalization;
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

        public string QuoteValues(IEnumerable<object> values)
        {
            return string.Join(_separator, values.Select(p => QuoteValue(Stringify(p))));
        }

        public static string Stringify(object o, IFormatProvider provider = null, string numericFormat = null, string datetimeFormat = null)
        {
            if (o is string s) return s;
            if (o is short sh) return sh.ToString();
            if (o is int i) return i.ToString();
            if (o is long l) return l.ToString();
            if (o is ushort ush) return ush.ToString();
            if (o is uint ui) return ui.ToString();
            if (o is ulong ul) return ul.ToString();
            if (o is float f) return f.ToString(numericFormat, provider ?? CultureInfo.InvariantCulture);
            if (o is double d) return d.ToString(numericFormat, provider ?? CultureInfo.InvariantCulture);
            if (o is decimal de) return de.ToString(numericFormat, provider ?? CultureInfo.InvariantCulture);
            if (o is DateTime dt) return dt.ToString(datetimeFormat ?? "yyyy-MM-dd HH:mm:ss.fff");
            throw new NotImplementedException("CsvWriter has no Stringify implementation for type " + o.GetType());
        }

        public void WriteLine(IEnumerable<string> values)
        {
            if (_targetFile == null) _targetFile = new StreamWriter(_targetPath);
            _targetFile.WriteLine(QuoteValues(values));
        }

        public void WriteAllLines(IEnumerable<IEnumerable<string>> contents)
        {
            using (var targetFile = _targetFile ?? new StreamWriter(_targetPath))
            {
                foreach (var line in contents)
                    targetFile.WriteLine(QuoteValues(line));
            }
        }

        public void WriteAllLines(IEnumerable<IEnumerable<object>> contents)
        {
            using (var targetFile = new StreamWriter(_targetPath))
            {
                foreach (var line in contents)
                    targetFile.WriteLine(QuoteValues(line));
            }
        }

        public static void Write(string targetPath, IEnumerable<IEnumerable<string>> contents, string separator = ";", string quote = "\"", string replaceNewLinesWith = " ")
        {
            new CsvWriter(targetPath, separator, quote, replaceNewLinesWith).WriteAllLines(contents);
        }

        public void Dispose()
        {
            _targetFile?.Dispose();
        }
    }
}
