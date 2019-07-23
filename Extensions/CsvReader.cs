using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public class CsvReader
    {
        private char _separator;
        private string _filename;
        private Dictionary<string, int> _headers = new Dictionary<string, int>();

        public CsvReader(string filename, char separator = ';')
        {
            _separator = separator;
            _filename = filename;
            _headers = System.IO.File.ReadLines(filename).First().Split(separator).Select((a, b) => new { Header = a, Index = b }).ToDictionary(k => k.Header, v => v.Index);
        }

        public IEnumerable<string[]> Read(params string[] headers)
        {
            var indices = headers.Select(c => _headers[c]).ToArray();
            return System.IO.File.ReadLines(_filename).Skip(1).Select(p => p.Split(_separator)).Select(p => indices.Select(c => p[c]).ToArray());
        }
    }
}
