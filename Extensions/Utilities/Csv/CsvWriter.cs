namespace Extensions.Utilities.Csv
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
}
