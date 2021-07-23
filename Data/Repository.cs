using Npgsql;
using System.Data;
using System.Threading.Tasks;

namespace RecipeApp.Data
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        private readonly string _connectionString;

        public Repository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection Open()
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            return connection;
        }
    }
}
