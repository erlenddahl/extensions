using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace NpgsqlExtensions.UnnestInserter
{
    public class UnnestInserter
    {
        public readonly List<IUnnestableColumn> UnnestColumns = new List<IUnnestableColumn>();
        private readonly List<IUnnestableColumn> _staticColumns = new List<IUnnestableColumn>();
        private IEnumerable<IUnnestableColumn> AllColumns => UnnestColumns.Concat(_staticColumns);

        public string TableName { get; set; }

        public string InsertTemplate { get; set; } = "INSERT INTO {0}({1}) VALUES({2})";

        public UnnestInserter(string tableName)
        {
            TableName = tableName;
        }

        public void Insert(NpgsqlConnection conn)
        {
            var cmdString = string.Format(InsertTemplate, TableName, GetNames(), GetParameters());
            Debug.WriteLine(cmdString);
            var cmd = new NpgsqlCommand(cmdString, conn);
            foreach(var col in AllColumns)
                col.AddParameters(cmd);
            cmd.ExecuteNonQuery();
        }

        public void Add(string key, IEnumerable<int> values)
        {
            UnnestColumns.Add(new UnnestableColumn<int>() { Name = key, Value = values.ToList() });
        }

        public void Add(string key, IEnumerable<double> values)
        {
            UnnestColumns.Add(new UnnestableColumn<double>() { Name = key, Value = values.ToList() });
        }

        public void Add(string key, IEnumerable<string> values)
        {
            UnnestColumns.Add(new UnnestableColumn<string>() { Name = key, Value = values.ToList() });
        }

        public void Add(string key, IEnumerable<DateTime> values)
        {
            UnnestColumns.Add(new UnnestableColumn<DateTime>() { Name = key, Value = values.ToList() });
        }

        public void AddStatic(string key, int value)
        {
            UnnestColumns.Add(new StaticColumn<int>() { Name = key, Value = value });
        }

        public void AddStatic(string key, double value)
        {
            UnnestColumns.Add(new StaticColumn<double>() { Name = key, Value = value });
        }

        public void AddStatic(string key, string value)
        {
            UnnestColumns.Add(new StaticColumn<string>() { Name = key, Value = value });
        }

        public void AddStatic(string key, DateTime value)
        {
            UnnestColumns.Add(new StaticColumn<DateTime>() { Name = key, Value = value });
        }

        private string GetNames()
        {
            return string.Join(", ", AllColumns.Select(p => p.Name));
        }

        private string GetParameters()
        {
            return string.Join(", ", AllColumns.Select(p => p.IsUnnestable ? "unnest(@" + p.Name + ")" : "@" + p.Name));
        }
    }

    public interface IUnnestableColumn
    {
        string Name { get; }
        bool IsUnnestable { get; }
        void AddParameters(NpgsqlCommand cmd);
    }

    public class UnnestableColumn<T> : IUnnestableColumn
    {
        public string Name { get; set; }
        public bool IsUnnestable => true;

        public List<T> Value { get; set; }
        public void AddParameters(NpgsqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@" + Name, Value.ToArray());
        }
    }

    public class StaticColumn<T> : IUnnestableColumn
    {
        public string Name { get; set; }
        public bool IsUnnestable => false;
        public T Value { get; set; }
        public void AddParameters(NpgsqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@" + Name, Value);
        }
    }
}
