using System;
using System.Threading.Tasks;

namespace RecipeApp.Data
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        Task<Guid> CreateAsync(ApplicationUser user);
        Task<ApplicationUser> GetAsync(Guid id);
        Task<int> UpdateAsync(ApplicationUser user);
        Task<int> DeleteAsync(Guid id);
        Task<ApplicationUser> GetByNameAsync(string normalizedUserName);
        Task<ApplicationUser> GetByEmailAsync(string normalizedEmail);
    }
}