using System;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Dapper;
using Npgsql;

namespace RecipeApp.Data
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        public UserRepository(NpgsqlConnection connection) : base(connection) {}

        public async Task<IdentityResult> CreateAsync(ApplicationUser user)
        {
            string sql = $@"INSERT INTO applicationuser (username, normalizedusername, email, normalizedemail, emailconfirmed, passwordhash) 
                            VALUES (@{nameof(ApplicationUser.UserName)}, @{nameof(ApplicationUser.NormalizedUserName)}, @{nameof(ApplicationUser.Email)},
                                    @{nameof(ApplicationUser.NormalizedEmail)}, @{nameof(ApplicationUser.EmailConfirmed)}, @{nameof(ApplicationUser.PasswordHash)}) 
                            RETURNING user_id";

            using (IDbConnection connection = Open())
            {
                int rows = await connection.ExecuteAsync(sql, user);

                if (rows > 0)
                {
                    return IdentityResult.Success;
                }
                return IdentityResult.Failed(new IdentityError { Description = $"Could not insert user {user.Email}." });
            }
        }
    }
}