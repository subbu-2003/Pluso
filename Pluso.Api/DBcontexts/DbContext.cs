using MySqlConnector;
using System.Data;

namespace ecom.DBcontexts
{
    public class DbContext : IDbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string? _mySqlconnectionString;

        public DbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _mySqlconnectionString = configuration.GetConnectionString("AuthDB");
        }

        public IDbConnection GetConnection()
        {
            return new MySqlConnection(_mySqlconnectionString);
        }
    }
}
