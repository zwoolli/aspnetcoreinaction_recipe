using RecipeApp.Models;
using Dapper;
using System.Collections.Generic;
using System;
using Npgsql;
using System.Threading.Tasks;
using System.Linq;

namespace RecipeApp.Data
{
    public class RecipeStore
    {
        private readonly string _connectionString;

        public RecipeStore(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<RecipeSummaryViewModel>> GetRecipes()
        {
            string sql = $@"SELECT recipeid, name, timecook, isdeleted, 
                                        method, isvegan, isvegetarian, lastmodified 
                            FROM recipe";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return (await connection.QueryAsync<RecipeSummaryViewModel>(sql)).ToList();;
            }
        }

        public async Task<bool> DoesRecipeExistAsync(int id) 
        {
            string sql = $@"SELECT * 
                            FROM recipe 
                            WHERE recipeId = @{nameof(id)}";

            // https://dapper-tutorial.net/query#example---query-multi-type
        }
    }
}