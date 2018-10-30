using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqliteExtensions
{
    public enum SQLiteColumnType
    {
        Int,
        Double,
        Boolean,
        DateTime,
        Text
    }

    internal class SQLiteColumn
    {
        public string Header { get; set; }
        public SQLiteColumnType Type { get; set; }
        public bool PrimaryKey { get; set; }
        public bool Nullable { get; set; }
        public string Default { get; set; }

        public string GetSQL()
        {
            var s = Header + " " + Type.ToString().ToUpper();
            if (!Nullable) s += " NOT NULL";
            return s;
        }
    }

    public class SQLiteTableBuilder
    {

        private List<SQLiteColumn> _columns = new List<SQLiteColumn>();
        private string _tableName;

        public SQLiteTableBuilder(string tableName)
        {
            _tableName = tableName;
        }

        public SQLiteTableBuilder AddColumn(string header, SQLiteColumnType type, bool primary = false, bool nullable = false, string defaultValue = null)
        {
            _columns.Add(new SQLiteColumn() { Header = header, Type = type, PrimaryKey = primary, Nullable = nullable, Default = defaultValue });
            return this;
        }

        public SQLiteTableBuilder Primary(params string[] headers)
        {
            foreach (var col in _columns)
                col.PrimaryKey = headers.Contains(col.Header);
            return this;
        }

        public void Create(SQLiteConnection conn, bool ifNotExists = true)
        {
            var hasPrimaryKey = _columns.Any(p => p.PrimaryKey);

            var s = "CREATE TABLE " + (ifNotExists ? "IF NOT EXISTS " : "") + _tableName + "(" + Environment.NewLine;
            s += "\t" + string.Join(Environment.NewLine + "\t", _columns.Select(p => p.GetSQL() + ","));
            if (hasPrimaryKey) s += Environment.NewLine + "\tPRIMARY KEY(" + string.Join(", ", _columns.Where(p => p.PrimaryKey).Select(p => p.Header)) + ")";
            else s = s.Substring(0, s.Length - 1);
            s += Environment.NewLine + ");";

            using (SQLiteCommand cmd = new SQLiteCommand(s, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}
