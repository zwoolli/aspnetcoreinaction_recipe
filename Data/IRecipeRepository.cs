using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using RecipeApp.Models;

namespace RecipeApp.Data
{
    public interface IRecipeRepository : IRepository<Recipe>
    {
        Task<IEnumerable<RecipeSummaryViewModel>> GetRecipesForSummary();
        Task<IEnumerable<RecipeSummaryViewModel>> GetRecipesByUser(Guid id);
        Task<Recipe> GetRecipeById(Guid id);
        Task<bool> DoesRecipeExistAsync(Guid id);
        Task<RecipeDetailViewModel> GetRecipeDetailAsync(Guid id);
        Task<UpdateRecipeCommand> GetRecipeForUpdateAsync(Guid id);
        Task<Guid> CreateRecipeAsync(CreateRecipeCommand cmd);
        Task UpdateRecipeAsync(UpdateRecipeCommand cmd);
        Task DeleteRecipeAsync(Guid id);
    }
}