using System;
using System.Collections.Generic;
using System.Linq;
using net.erlenddahl.Extensions.StringExtensions;
using Npgsql;

namespace net.erlenddahl.NpgsqlExtensions.Orm
{
    public class PostgresOrmTable
    {
        public List<PostgresOrmColumn> Columns { get; }
        public PostgresOrmColumn IdColumn { get; private set; }

        public bool HasId => IdColumn != null;

        public string ColumnString { get; private set; }
        public string ParameterString { get; private set; }

        public string ColumnStringWithoutId { get; private set; }
        public string ParameterStringWithoutId { get; private set; }

        private readonly NpgsqlCommandBuilder _cmdBuilder;

        public PostgresOrmTable(Type t)
        {
            _cmdBuilder = new NpgsqlCommandBuilder();
            Columns = t.GetProperties().Select(p => new PostgresOrmColumn(p)).ToList();
            ApplyColumnChanges();
        }

        public void ApplyColumnChanges()
        {
            IdColumn = Columns.FirstOrDefault(p => p.IsIdColumn);

            ColumnString = string.Join(",", Columns.Select(p => _cmdBuilder.QuoteIdentifier(p.Name)));
            ParameterString = string.Join(",", Columns.Select(p => p.QueryValue));

            ColumnStringWithoutId = string.Join(",", Columns.Where(p => !p.IsIdColumn).Select(p => _cmdBuilder.QuoteIdentifier(p.Name)));
            ParameterStringWithoutId = string.Join(",", Columns.Where(p => !p.IsIdColumn).Select(p => p.QueryValue));
        }

        public string GetValueString<T>(T element)
        {
            return string.Join(",", GetValues(element));
        }

        public object[] GetValues<T>(T element)
        {
            return Columns.Select(p => p.GetValue(element)).ToArray();
        }

        public NpgsqlCommand GetInsertQuery(string tableName, NpgsqlConnection conn = null)
        {
            return new NpgsqlCommand("INSERT INTO public." + _cmdBuilder.QuoteIdentifier(tableName) + "(" + ColumnStringWithoutId + ") VALUES(" + ParameterStringWithoutId + ")", conn);
        }

        public NpgsqlCommand GetSelectQuery(string tableName, NpgsqlConnection conn = null)
        {
            return new NpgsqlCommand("SELECT " + ColumnString + " FROM public." + _cmdBuilder.QuoteIdentifier(tableName), conn);
        }

        public NpgsqlCommand GetPopulatedInsertQuery<T>(string tableName, T element, NpgsqlConnection conn = null)
        {
            var cmd = GetInsertQuery(tableName, conn);
            foreach (var p in Columns)
                if (p.StaticValue == null)
                    cmd.Parameters.AddWithValue("@" + p.Name, p.GetValue(element) ?? DBNull.Value);
            return cmd;
        }

        public NpgsqlCommand GetCreationQuery(string tableName, string ownerUsername = null, string createIdColumn = null, NpgsqlConnection conn = null, bool dropIfExists = false)
        {
            return GetCreationQuery(tableName, Columns, p => p.ToCamelCase(), ownerUsername, createIdColumn, conn, dropIfExists);
        }

        public static NpgsqlCommand GetCreationQuery(string tableName, IEnumerable<PostgresOrmColumn> columns, Func<string, string> columnNameAdjuster, string ownerUsername = null, string createIdColumn = null, NpgsqlConnection conn = null, bool dropIfExists = false)
        {
            var cmd = "";

            if (dropIfExists)
                cmd += "DROP TABLE IF EXISTS public." + tableName + ";";

            cmd += "CREATE TABLE public." + tableName + "(";
            var isFirst = true;

            if (createIdColumn != null)
            {
                cmd += createIdColumn + " SERIAL NOT NULL";
                isFirst = false;
            }

            var b = new NpgsqlCommandBuilder();
            foreach (var col in columns)
            {
                cmd += (isFirst ? "" : ", ") + b.QuoteIdentifier(columnNameAdjuster(col.Name)) + " " + (col.IsIdColumn ? "SERIAL NOT NULL" : col.TypeName);
                isFirst = false;
            }

            cmd += ") WITH (OIDS = FALSE);";

            if (ownerUsername != null)
                cmd += "ALTER TABLE public." + tableName + " OWNER to " + ownerUsername + ";";

            return new NpgsqlCommand(cmd, conn);
        }

        internal void InsertAndUpdateId<T>(string tableName, T element, NpgsqlConnection conn)
        {
            var cmd = GetPopulatedInsertQuery(tableName, element, conn);

            if (HasId)
                cmd.CommandText += " RETURNING id";

            var id = cmd.ExecuteScalar();

            if (HasId)
                IdColumn.SetValue(element, id);
        }

        public string Quote(string column)
        {
            return _cmdBuilder.QuoteIdentifier(column);
        }
    }
}