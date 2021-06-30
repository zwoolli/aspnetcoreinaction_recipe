using RecipeApp.Models;
using Dapper;
using System.Collections.Generic;
using System;
using System.Data;
using Npgsql;
using System.Threading.Tasks;
using System.Linq;

namespace RecipeApp.Data
{
    public class RepiceRepository : Repository<Recipe>, IRecipeRepository
    {        

        public RepiceRepository(NpgsqlConnection connection) : base(connection) {}

        public async Task<List<Recipe>> GetRecipesAsync()
        {
            string sql = $@"SELECT * 
                            FROM recipe 
                            INNER JOIN ingredient 
                            ON (recipe.recipe_id = ingredient.recipe_id)";

            using (IDbConnection connection = Open())
            {
                Dictionary<Guid, Recipe> recipeDictionary = new Dictionary<Guid, Recipe>();

                List<Recipe> recipes = (
                    await connection.QueryAsync<Recipe, Ingredient, Recipe>(
                        sql,
                        (recipe, ingredient) =>
                        {
                            Recipe recipeEntry;

                            if (!recipeDictionary.TryGetValue(recipe.Recipe_Id, out recipeEntry))
                            {
                                recipeEntry = recipe;
                                recipeEntry.Ingredients = new List<Ingredient>();
                                recipeDictionary.Add(recipeEntry.Recipe_Id, recipeEntry);
                            }

                            recipeEntry.Ingredients.Add(ingredient);
                            return recipeEntry;
                        },
                        splitOn: "ingredient_id"
                    )
                )
                .Distinct()
                .ToList();

                return recipes;
            }
        }

        public async Task<Recipe> GetRecipeAsync(Guid id)
        {
            string sql = $@"SELECT * 
                            FROM recipe
                            INNER JOIN ingredient
                            ON (recipe.recipe_id = ingredient.recipe_id)
                            WHERE recipe.recipe_id = @{nameof(id)}";

            using (IDbConnection connection = Open())
            {
                Dictionary<Guid, Recipe> recipeDictionary = new Dictionary<Guid, Recipe>();

                List<Recipe> recipes = (
                    await connection.QueryAsync<Recipe, Ingredient, Recipe>(
                        sql,
                        (recipe, ingredient) =>
                        {
                            Recipe recipeEntry;

                            if (!recipeDictionary.TryGetValue(recipe.Recipe_Id, out recipeEntry))
                            {
                                recipeEntry = recipe;
                                recipeEntry.Ingredients = new List<Ingredient>();
                                recipeDictionary.Add(recipeEntry.Recipe_Id, recipeEntry);
                            }

                            recipeEntry.Ingredients.Add(ingredient);
                            return recipeEntry;
                        },
                        param: new {id},
                        splitOn: "ingredient_id"
                    )
                )
                .Distinct()
                .ToList();
// make sure there is only one in this list
                return recipes[0];
            }
        }

        public async Task<List<RecipeSummaryViewModel>> GetRecipesForSummary()
        {
            IEnumerable<Recipe> recipes;
            List<RecipeSummaryViewModel> recipeViewModels;

            string sql = $@"SELECT *
                            FROM recipe";

            using (IDbConnection connection = Open())
            {
                recipes = await connection.QueryAsync<Recipe>(sql);
            }

            recipeViewModels = recipes.Select(r => 
                new RecipeSummaryViewModel
                {
                    Id = r.Recipe_Id,
                    Name = r.Name,
                    TimeToCook = $"{r.TimeToCook.Hours}hrs {r.TimeToCook.Minutes}mins",
                }).ToList();

            return recipeViewModels;
        }

        public async Task<bool> DoesRecipeExistAsync(Guid id) 
        {
            string sql = $@"SELECT * 
                            FROM recipe 
                            WHERE recipe_Id = @{nameof(id)}";


            using (IDbConnection connection = Open())
            {
                Recipe recipe = await connection.QuerySingleOrDefaultAsync<Recipe>(sql, new {id});
                
                return recipe == null ? false : true;    
            }         
        }

        public async Task<RecipeDetailViewModel> GetRecipeDetailAsync(Guid id)
        {
            Recipe recipe;
            RecipeDetailViewModel recipeDetailViewModel;

            string sql = $@"SELECT * 
                            FROM recipe
                            WHERE recipe_Id = @{nameof(id)}";

            using (IDbConnection connection = Open())
            {
                recipe = await connection.QuerySingleOrDefaultAsync<Recipe>(sql, new {id});
            }

            recipeDetailViewModel = new RecipeDetailViewModel 
            {
                Id = recipe.Recipe_Id,
                Name = recipe.Name,
                Method = recipe.Method,
                Ingredients = recipe.Ingredients.Select(i => 
                    new RecipeDetailViewModel.Item
                    {
                        Name = i.Name,
                        Quantity = $"{i.Quantity} {i.Unit}"
                    })
            };

            return recipeDetailViewModel;
        }

        public async Task<UpdateRecipeCommand> GetRecipesForUpdateAsync(Guid id)
        {
            Recipe recipe;
            UpdateRecipeCommand updateRecipeCommand;

            string sql = $@"SELECT * 
                            FROM recipe
                            WHERE recipe_Id = @{nameof(id)}";

            using (IDbConnection connection = Open())
            {
                recipe = await connection.QuerySingleOrDefaultAsync<Recipe>(sql, new {id});
            }

            updateRecipeCommand =
                new UpdateRecipeCommand
                {
                    Name = recipe.Name,
                    Method = recipe.Method,
                    TimeToCookHrs = recipe.TimeToCook.Hours,
                    TimeToCookMins = recipe.TimeToCook.Minutes,
                    IsVegan = recipe.IsVegan,
                    IsVegetarian = recipe.IsVegetarian
                };

            return updateRecipeCommand;
        }

        public async Task<Guid> CreateRecipeAsync(CreateRecipeCommand cmd)
        {
            Recipe recipe = cmd.ToRecipe();
            //TODO: figure out how to insert list of ingredients too
            string sql = $@"INSERT INTO recipe (recipe_Id, name, timeToCook, 
                                                method, isVegan, isVegetarian, lastModified) 
                            VALUES (@{nameof(recipe.Recipe_Id)}, @{nameof(recipe.Name)}, @{nameof(recipe.TimeToCook)}, 
                                        @{nameof(recipe.Method)}, @{nameof(recipe.IsVegan)}, 
                                        @{nameof(recipe.IsVegetarian)}, @{nameof(recipe.LastModified)}) 
                            RETURNING recipe_Id";

            using (IDbConnection connection = Open())
            {
                //TODO: figure out how to return guid from executeasync
                return await connection.ExecuteAsync(sql, recipe);
            }
        }
    }
}