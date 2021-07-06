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

        public RecipeRepository(NpgsqlConnection connection) : base(connection) {}

        public async Task<IEnumerable<Recipe>> GetRecipesAsync()
        {
            string sql = $@"SELECT * 
                            FROM recipe 
                            INNER JOIN ingredient 
                            ON (recipe.recipe_id = ingredient.recipe_id)";

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

                            recipeEntry.Ingredients.Add(ingredient);
                            return recipeEntry;
                        },
                        splitOn: "ingredient_id"
                    )
                )
                .Distinct();

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

                Recipe recipe = (
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
                .FirstOrDefault();

                return recipe;
            }
        }

        public async Task<IEnumerable<RecipeSummaryViewModel>> GetRecipesForSummary()
        {
            IEnumerable<Recipe> recipes = await GetRecipesAsync();

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
            Recipe recipe = await GetRecipeAsync(id);
            return recipe == null ? false : true;         
        }

        public async Task<RecipeDetailViewModel> GetRecipeDetailAsync(Guid id)
        {
            Recipe recipe = await GetRecipeAsync(id);

            RecipeDetailViewModel recipeDetailViewModel = new RecipeDetailViewModel 
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

        public async Task<UpdateRecipeCommand> GetRecipeForUpdateAsync(Guid id)
        {
            Recipe recipe = await GetRecipeAsync(id);

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
            
            string sqlRecipe = $@"INSERT INTO recipe (recipe_Id, name, timeToCook, 
                                                method, isVegan, isVegetarian, lastModified) 
                                VALUES (@{nameof(Recipe.Recipe_Id)}, @{nameof(Recipe.Name)}, @{nameof(Recipe.TimeToCook)}, 
                                            @{nameof(Recipe.Method)}, @{nameof(Recipe.IsVegan)}, 
                                            @{nameof(Recipe.IsVegetarian)}, @{nameof(Recipe.LastModified)})";

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
            Recipe recipe = await GetRecipeAsync(cmd.Id);
            if (recipe == null) { throw new Exception("Unable to find the recipe"); }

            cmd.UpdateRecipe(recipe);

            string sql = $@"UPDATE recipe 
                            SET name = @{nameof(Recipe.Name)},
                                timeToCook = @{nameof(Recipe.TimeToCook)},
                                method = @{nameof(Recipe.Method)},
                                isVegetarian = @{nameof(Recipe.IsVegetarian)},
                                isVegan = @{nameof(Recipe.IsVegan)} 
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