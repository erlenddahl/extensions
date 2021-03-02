using System.Collections.Generic;
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
}