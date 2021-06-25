using System.Threading.Tasks;
using Npgsql;
using System.Data;

namespace RecipeApp.Data
{
    public interface IRepository<T>
    {
        IDbConnection Open();
    }
}
