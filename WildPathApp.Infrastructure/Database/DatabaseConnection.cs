using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;

namespace WildPathApp.Infrastructure.Database;

//Try catch block may be added here | Or enable global exception handling
public class DatabaseConnection
{
    private readonly string _connectionString;
    public DatabaseConnection(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }

}
