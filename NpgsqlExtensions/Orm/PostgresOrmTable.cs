using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Npgsql;

namespace NpgsqlExtensions.Orm
{
    public class PostgresOrmTable
    {
        public PostgresOrmColumn[] Columns { get; }
        public PostgresOrmColumn IdColumn { get; }

        public bool HasId => IdColumn != null;

        public string ColumnString { get; }
        public string ParameterString { get; }

        public string ColumnStringWithoutId { get; }
        public string ParameterStringWithoutId { get; }

        private readonly NpgsqlCommandBuilder _cmdBuilder;

        public PostgresOrmTable(Type t)
        {
            _cmdBuilder = new NpgsqlCommandBuilder();
            var properties = t.GetProperties();
            Columns = properties.Select(p => new PostgresOrmColumn(p)).ToArray();

            IdColumn = Columns.FirstOrDefault(p => p.IsIdColumn);

            ColumnString = string.Join(",", Columns.Select(p => _cmdBuilder.QuoteIdentifier(p.Name)));
            ParameterString = string.Join(",", Columns.Select(p => "@" + p.Name));

            ColumnStringWithoutId = string.Join(",", Columns.Where(p => !p.IsIdColumn).Select(p => _cmdBuilder.QuoteIdentifier(p.Name)));
            ParameterStringWithoutId = string.Join(",", Columns.Where(p => !p.IsIdColumn).Select(p => "@" + p.Name));
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
                cmd.Parameters.AddWithValue("@" + p.Name, p.GetValue(element) ?? DBNull.Value);
            return cmd;
        }

        public NpgsqlCommand GetCreationQuery(string tableName, string ownerUsername = null, string createIdColumn = null, NpgsqlConnection conn = null, bool dropIfExists = false)
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

            foreach (var col in Columns)
            {
                cmd += (isFirst ? "" : ", ") + _cmdBuilder.QuoteIdentifier(col.Name.ToCamelCase()) + " " + (col.IsIdColumn ? "SERIAL NOT NULL" : col.TypeName);
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