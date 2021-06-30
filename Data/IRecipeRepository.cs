using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using RecipeApp.Models;

namespace RecipeApp.Data
{
    public interface IRecipeRepository : IRepository<Recipe>
    {
        Task<List<Recipe>> GetRecipesAsync();
        Task<Recipe> GetRecipeAsync(Guid id);
        Task<List<RecipeSummaryViewModel>> GetRecipesForSummary();
        Task<bool> DoesRecipeExistAsync(Guid id);
        Task<RecipeDetailViewModel> GetRecipeDetailAsync(Guid id);
        Task<UpdateRecipeCommand> GetRecipeForUpdateAsync(Guid id);
        Task<Guid> CreateRecipeAsync(CreateRecipeCommand cmd);
        Task UpdateRecipe(UpdateRecipeCommand cmd);
        Task DeleteRecipe(Guid id);
    }
}