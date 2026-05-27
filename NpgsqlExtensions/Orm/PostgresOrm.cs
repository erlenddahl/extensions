using Npgsql;

namespace net.erlenddahl.NpgsqlExtensions.Orm
{
    public class PostgresOrm
    {

        private readonly string _connString;
        private readonly string _username;

        public PostgresOrm(string host, int port, string database, string username, string password)
        {
            _connString = new NpgsqlConnectionStringBuilder()
            {
                Host = host,
                Port = port,
                Database = database,
                Username = username,
                Password = password
            }.ToString();
            _username = username;
        }

        public PostgresOrmSession OpenSession()
        {
            return new PostgresOrmSession(_connString, _username);
        }
    }
}
