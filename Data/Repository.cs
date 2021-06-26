using Npgsql;
using System.Data;
using System.Threading.Tasks;

namespace RecipeApp.Data
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        private readonly IDbConnection _connection;

        public Repository(IDbConnection connection)
        {
            _connection = connection;
        }

        public IDbConnection Open()
        {
            _connection.Open();

            return _connection;
        }
    }
}
