using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Extensions.StringExtensions;

namespace Extensions.Utilities.Csv
{
    public class CsvSettings
    {
        public char Separator { get; set; } = ';';
        public char Quote { get; set; } = '"';
        public bool HasHeaders { get; set; } = true;
        public bool LowercaseHeaders { get; set; } = false;
        public Encoding Encoding { get; set; } = Encoding.UTF8;
        public bool Debug { get; set; } = false;
    }

    public class CsvReader
    {
        public CsvSettings Settings { get; set; }

        public CsvReader(CsvSettings settings = null)
        {
            Settings = settings ?? new CsvSettings();
        }

        public static IEnumerable<CsvRow> FromFile(string filename, CsvSettings settings = null)
        {
            return new CsvReader(settings).ReadFile(filename);
        }

        public static IEnumerable<CsvRow> FromString(string csvString, CsvSettings settings = null)
        {
            return new CsvReader(settings).ReadString(csvString);
        }

        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static IEnumerable<CsvRow> FromLines(IEnumerable<string> lines, CsvSettings settings = null)
        {
            return new CsvReader(settings).ReadLines(lines);
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
                    yield return row.Substring(currStart, i - currStart).Trim(Settings.Quote);
                    currStart = i + 1;
                }
            }

            if (currStart <= row.Length)
                yield return row.Substring(currStart, row.Length - currStart).Trim(Settings.Quote);
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
            if (c == Settings.Quote)
                insideQuotes = !insideQuotes;
            else if (!insideQuotes && c == Settings.Separator)
                return true;

            return false;
        }

        enum SeparatorType
        {
            None,
            Column,
            Line
        }

        private SeparatorType IsSeparator(char c, ref bool insideQuotes)
        {
            if (c == Settings.Quote)
                insideQuotes = !insideQuotes;
            else if (!insideQuotes && c == Settings.Separator)
                return SeparatorType.Column;
            else if (!insideQuotes && c == '\n')
                return SeparatorType.Line;

            return SeparatorType.None;
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
                    if (currentColumn == column) return row.Substring(currStart, i - currStart).Trim(Settings.Quote);
                    currentColumn++;
                    currStart = i + 1;
                }
            }

            if (currStart <= row.Length && currentColumn == column)
                return row.Substring(currStart, row.Length - currStart).Trim(Settings.Quote);

            throw new Exception($"Column {column:n0} not found.");
        }

        public IEnumerable<CsvRow> ReadFile(string filename)
        {
            return ReadStream(File.OpenRead(filename));
        }

        private IEnumerable<CsvRow> ReadStream(Stream stream)
        {
            var decoder = Settings.Encoding.GetDecoder();
            var inputBuffer = new byte[1];

            var insideQuotes = false;
            var isEscaped = false;

            var headers = new Dictionary<string, int>();
            var current = new List<string>();
            var isReadingHeaders = Settings.HasHeaders;

            var currentColumn = 0;
            var sb = new StringBuilder();
            var sbDebug = new StringBuilder();
            var previousCharacter = char.MinValue;

            while (stream.Read(inputBuffer, 0, 1) > 0)
            {
                var charCount = decoder.GetCharCount(inputBuffer, 0, 1);
                if (charCount < 1)
                {
                    if (Settings.Debug)
                    {
                        sbDebug.AppendLine("Read zero-character");
                    }
                    continue;
                }

                // Read a character (that may be more than a single-character)
                var readChars = new char[charCount];
                decoder.GetChars(inputBuffer, 0, 1, readChars, 0);

                if (Settings.Debug)
                {
                    sbDebug.Append(readChars);
                    sbDebug.Append($" [{string.Join(", ", readChars.Select(p => (int)p))}], isQuote={readChars[0] == Settings.Quote}");
                }

                if (previousCharacter == '"' && readChars[0] == '"')
                {
                    insideQuotes = !insideQuotes;
                    if (Settings.Debug) sbDebug.AppendLine(", isDoubleQuote");
                    continue;
                }

                previousCharacter = readChars[0];

                if (CheckEscape(readChars[0], ref isEscaped))
                {
                    if (Settings.Debug) sbDebug.AppendLine(", isEscapeCharacter");
                    continue;
                }

                var separator = IsSeparator(readChars[0], ref insideQuotes);

                if (Settings.Debug) sbDebug.AppendLine($", isInsideQuotes={insideQuotes}, separator={separator}");

                if (separator != SeparatorType.None)
                {
                    HandleValueEnd(sb, current, separator, headers, isReadingHeaders, currentColumn);
                    if (separator == SeparatorType.Column)
                    {
                        currentColumn++;
                    }
                    else
                    {
                        currentColumn = 0;
                        if (!isReadingHeaders)
                        {
                            yield return new CsvRow(current, headers){ Debug = sbDebug.ToString() };
                        }
                        
                        isReadingHeaders = false;
                        current.Clear();
                        sbDebug.Clear();
                    }

                    sb.Clear();
                    continue;
                }

                sb.Append(readChars);
            }

            if (currentColumn > 0)
            {
                HandleValueEnd(sb, current, SeparatorType.Line, headers, isReadingHeaders, currentColumn);
                yield return new CsvRow(current, headers);
            }
        }

        private void HandleValueEnd(StringBuilder sb, List<string> current, SeparatorType separator, Dictionary<string, int> headers, bool isReadingHeaders, int currentColumn)
        {
            var value = sb.ToString().Trim(Settings.Quote, '\r');

            if (isReadingHeaders)
            {
                headers[value] = currentColumn;
            }
            else
            {
                current.Add(value);
            }
        }

        public IEnumerable<CsvRow> ReadLines(IEnumerable<string> lines)
        {
            var headers = new Dictionary<string, int>();
            if (Settings.HasHeaders)
                headers = ParseHeaders(lines.First());
            return lines.Skip(Settings.HasHeaders ? 1 : 0).Select(SplitRow).Where(p => p.Any()).Select(p => new CsvRow(p, headers));
        }

        public IEnumerable<CsvRow> ReadString(string csvString)
        {
            return ReadStream(GenerateStreamFromString(csvString));
        }

        public Dictionary<string, int> ReadHeaders(string filename)
        {
            return ParseHeaders(System.IO.File.ReadLines(filename).First());
        }

        private Dictionary<string, int> ParseHeaders(string headerRow)
        {
            return SplitRow(headerRow).Select((a, b) => new {Header = a, Index = b}).ToDictionary(k => Settings.LowercaseHeaders ? k.Header.ToLower() : k.Header, v => v.Index);
        }
    }
}