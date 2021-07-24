using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace RecipeApp.Data
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        public UserRepository(string connectionString) : base(connectionString) {}

        public async Task<Guid> CreateAsync(ApplicationUser user)
        {
            string sql = $@"INSERT INTO applicationuser (username, normalizedusername, email, normalizedemail, emailconfirmed, passwordhash) 
                            VALUES (@{nameof(ApplicationUser.UserName)}, @{nameof(ApplicationUser.NormalizedUserName)}, @{nameof(ApplicationUser.Email)},
                                    @{nameof(ApplicationUser.NormalizedEmail)}, @{nameof(ApplicationUser.EmailConfirmed)}, @{nameof(ApplicationUser.PasswordHash)}) 
                            RETURNING user_id";

            using (IDbConnection connection = Open())
            {
                return await connection.QuerySingleAsync<Guid>(sql, user);
            }
        }

        public async Task<ApplicationUser> GetAsync(Guid id)
        {
            string sql = $@"SELECT * 
                            FROM applicationuser 
                            WHERE user_id = @{nameof(id)}";

            using (IDbConnection connection = Open())
            {
                return await connection.QuerySingleOrDefaultAsync<ApplicationUser>(sql, new {id});
            }
        }

        public async Task<int> UpdateAsync(ApplicationUser user)
        {
            string sql = $@"UPDATE applicationuser 
                            SET username = @{nameof(ApplicationUser.UserName)},
                                normalizedusername = @{nameof(ApplicationUser.NormalizedUserName)},
                                email = @{nameof(ApplicationUser.Email)},
                                normalizedemail = @{nameof(ApplicationUser.NormalizedEmail)},
                                emailconfirmed = @{nameof(ApplicationUser.EmailConfirmed)},
                                passwordhash = @{nameof(ApplicationUser.PasswordHash)} 
                            WHERE user_id = @{nameof(ApplicationUser.User_Id)}";

            using (IDbConnection connection = Open())
            {
                return await connection.ExecuteAsync(sql, user);
            }
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            string sql = $@"DELETE 
                            FROM applicationuser 
                            WHERE user_id = @{nameof(id)}";

            using (IDbConnection connection = Open())
            {
               return await connection.ExecuteAsync(sql, new {id});
            }            
        }

        public async Task<ApplicationUser> GetByNameAsync(string normalizedusername)
        {
            string sql = $@"SELECT * 
                            FROM applicationuser 
                            WHERE normalizedusername = @{nameof(normalizedusername)}";

            using (IDbConnection connection = Open())
            {
                return await connection.QuerySingleOrDefaultAsync<ApplicationUser>(sql, new {normalizedusername});
            }
        }

        public async Task<ApplicationUser> GetByEmailAsync(string normalizedEmail)
        {
            string sql = $@"SELECT * 
                            FROM applicationuser 
                            WHERE normalizedemail = @{nameof(ApplicationUser.NormalizedEmail)}";

            using (IDbConnection connection = Open())
            {
                return await connection.QuerySingleOrDefaultAsync<ApplicationUser>(sql, new { normalizedEmail });
            }
        }
    }
}