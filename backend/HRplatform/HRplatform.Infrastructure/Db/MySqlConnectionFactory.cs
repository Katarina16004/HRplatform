using MySqlConnector;

namespace HRplatform.Infrastructure.Db
{
    public class MySqlConnectionFactory
    {
        private readonly string _connectionString;

        // connection from settings
        public MySqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        // creating MySqlConn object for using connection
        public MySqlConnection Create() => new MySqlConnection(_connectionString);
    }
}