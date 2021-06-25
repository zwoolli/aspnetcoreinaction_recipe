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
            // TODO: Look at ASP.NET Core way of injecting new connection
            //https://github.com/dotnet/AspNetCore.Docs/blob/main/aspnetcore/security/authentication/identity-custom-storage-providers/sample/CustomIdentityProviderSample/Startup.cs
            _connection.Open();

            return _connection;
        }
    }
}
