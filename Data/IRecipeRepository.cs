using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using RecipeApp.Models;

namespace RecipeApp.Data
{
    public interface IRecipeRepository : IRepository<Recipe>
    {
        Task<Guid> CreateRecipeAsync(CreateRecipeCommand cmd);
        Task<List<RecipeSummaryViewModel>> GetRecipes(Guid id);
        Task<RecipeDetailViewModel> GetRecipeDetail(Guid id);
        Task<UpdateRecipeCommand> GetRecipesForUpdate(Guid id);
        Task UpdateAsync(Recipe user);
        Task DeleteAsync(Guid id);
    }
}