using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace ClinicApp.Data
{
    public class ClinicAppMySqlDbContext: DbContext
    {
        private readonly string _connectionString;

        public ClinicAppMySqlDbContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public MySqlConnection GetConnection() => new MySqlConnection(_connectionString);
    }
}