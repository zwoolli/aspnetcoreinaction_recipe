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
    public class RecipeRepository : Repository<Recipe>, IRecipeRepository
    {        

        public RecipeRepository(string connectionString) : base(connectionString) {}
        private async Task<IEnumerable<Recipe>> GetRecipesAsync(string sql, Guid id = new Guid())
        {
            using (IDbConnection connection = Open())
            {
                Dictionary<Guid, Recipe> recipeDictionary = new Dictionary<Guid, Recipe>();

                IEnumerable<Recipe> recipes = (
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

                            if (ingredient != null) recipeEntry.Ingredients.Add(ingredient);
                            return recipeEntry;
                        },
                        param: new {id},
                        splitOn: "ingredient_id"
                    )
                )
                .Distinct();
            
                return recipes;
            }
        }
        private async Task<IEnumerable<Recipe>> GetAllRecipesAsync()
        {
            string sql = $@"SELECT * 
                            FROM recipe 
                            LEFT JOIN ingredient 
                            ON (recipe.recipe_id = ingredient.recipe_id)";

            IEnumerable<Recipe> recipes = await GetRecipesAsync(sql);
            
            return recipes;
        }

        public async Task<Recipe> GetRecipeById(Guid id)
        {
            string sql = $@"SELECT * 
                            FROM recipe
                            LEFT JOIN ingredient
                            ON (recipe.recipe_id = ingredient.recipe_id)
                            WHERE recipe.recipe_id = @{nameof(id)}";

            Recipe recipe = (await GetRecipesAsync(sql, id)).FirstOrDefault();
            return recipe;
        }

        public async Task<IEnumerable<RecipeSummaryViewModel>> GetRecipesByUser(Guid id)
        {
            string sql = $@"SELECT * 
                            FROM recipe
                            LEFT JOIN ingredient
                            ON (recipe.recipe_id = ingredient.recipe_id)
                            WHERE recipe.user_id = @{nameof(id)}";

            IEnumerable<Recipe> recipes = await GetRecipesAsync(sql, id);

            IEnumerable<RecipeSummaryViewModel> recipeViewModels = recipes.Select(r => 
                new RecipeSummaryViewModel
                {
                    Id = r.Recipe_Id,
                    Name = r.Name,
                });

            return recipeViewModels;
        }

        public async Task<IEnumerable<RecipeSummaryViewModel>> GetRecipesForSummary()
        {
            IEnumerable<Recipe> recipes = await GetAllRecipesAsync();

            IEnumerable<RecipeSummaryViewModel> recipeViewModels = recipes.Select(r => 
                new RecipeSummaryViewModel
                {
                    Id = r.Recipe_Id,
                    Name = r.Name,
                    TimeToCook = $"{r.TimeToCook.Hours}hrs {r.TimeToCook.Minutes}mins",
                });

            return recipeViewModels;
        }

        public async Task<bool> DoesRecipeExistAsync(Guid id) 
        {
            Recipe recipe = await GetRecipeById(id);
            return recipe == null ? false : true;         
        }

        public async Task<RecipeDetailViewModel> GetRecipeDetailAsync(Guid id)
        {
            Recipe recipe = await GetRecipeById(id);

            RecipeDetailViewModel recipeDetailViewModel = new RecipeDetailViewModel 
            {
                Id = recipe.Recipe_Id,
                Name = recipe.Name,
                Method = recipe.Method,
                Ingredients = recipe.Ingredients?.Select(i => 
                    new RecipeDetailViewModel.Item
                    {
                        Name = i.Name,
                        Quantity = $"{i.Quantity} {i.Unit}"
                    })
            };

            return recipeDetailViewModel;
        }

        public async Task<UpdateRecipeCommand> GetRecipeForUpdateAsync(Guid id)
        {
            Recipe recipe = await GetRecipeById(id);

            UpdateRecipeCommand updateRecipeCommand =
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

            string sqlRecipe = $@"INSERT INTO recipe (recipe_Id, user_Id, name, timeToCook, 
                                                method, isVegan, isVegetarian) 
                                VALUES (@{nameof(Recipe.Recipe_Id)}, @{nameof(Recipe.User_Id)}, @{nameof(Recipe.Name)}, @{nameof(Recipe.TimeToCook)}, 
                                            @{nameof(Recipe.Method)}, @{nameof(Recipe.IsVegan)}, 
                                            @{nameof(Recipe.IsVegetarian)})";

            string sqlIngredient = $@"INSERT INTO ingredient (ingredient_id, recipe_id, name, quantity, unit) 
                                    VALUES (@{nameof(Ingredient.Ingredient_Id)}, @{nameof(Ingredient.Recipe_Id)}, @{nameof(Ingredient.Name)},
                                            @{nameof(Ingredient.Quantity)}, @{nameof(Ingredient.Unit)})";

            using (IDbConnection connection = Open())
            {
                await connection.ExecuteAsync(sqlRecipe, recipe);
                await connection.ExecuteAsync(sqlIngredient, recipe.Ingredients);

                return recipe.Recipe_Id;
            }
        }

        public async Task UpdateRecipeAsync(UpdateRecipeCommand cmd)
        {
            Recipe recipe = cmd.UpdateRecipe();
            
            if (recipe == null) { throw new Exception("Unable to find the recipe"); }

            string sql = $@"UPDATE recipe 
                            SET name = @{nameof(Recipe.Name)},
                                timeToCook = @{nameof(Recipe.TimeToCook)},
                                method = @{nameof(Recipe.Method)},
                                isVegan = @{nameof(Recipe.IsVegan)},
                                isVegetarian = @{nameof(Recipe.IsVegetarian)}
                            WHERE recipe_id = @{nameof(Recipe.Recipe_Id)}";

            using (IDbConnection connection = Open())
            {
                await connection.ExecuteAsync(sql, recipe);
            }
        }

        public async Task DeleteRecipeAsync(Guid id)
        {
            string sqlRecipe = $@"DELETE 
                                FROM recipe 
                                WHERE recipe_id = @{nameof(id)}";

            string sqlIngredient = $@"DELETE 
                                    FROM ingredient 
                                    WHERE recipe_id = @{nameof(id)}";

            using (IDbConnection connection = Open())
            {
                await connection.ExecuteAsync(sqlIngredient, new {id});
                await connection.ExecuteAsync(sqlRecipe, new {id});
            }
        }
    }
}