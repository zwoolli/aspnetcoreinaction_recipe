using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace RecipeApp.Data
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        Task<IdentityResult> CreateAsync(ApplicationUser user);
        Task<IEnumerable<ApplicationUser>> GetAllAsync();
        Task<ApplicationUser> GetAsync(Guid id);
        Task UpdateAsync(ApplicationUser user);
        Task DeleteAsync(Guid id);
        Task<ApplicationUser> GetByNameAsync(string normalizedUserName);
        Task<ApplicationUser> GetByEmailAsync(string normalizedEmail);
    }
}