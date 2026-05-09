using MySqlConnector;

namespace HRplatform.Infrastructure.Db
{
    public class MySqlConnectionFactory
    {
        private readonly string _connectionString;

        public MySqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public MySqlConnection Create() => new MySqlConnection(_connectionString);
    }
}