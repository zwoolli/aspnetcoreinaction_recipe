using System;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeApp.Models;
using RecipeApp.Data;
using Microsoft.Extensions.Logging;

namespace RecipeApp.Pages.Recipes
{
    public class ViewModel : PageModel
    {
        private readonly IRecipeRepository _repository;
        private readonly IAuthorizationService _authService;
        private readonly ILogger<ViewModel> _log;
        public RecipeDetailViewModel Recipe { get; set; }
        public bool CanEditRecipe { get; set; }

        public ViewModel(IRecipeRepository repository, IAuthorizationService authService, ILogger<ViewModel> log)
        {
            _repository = repository;
            _authService = authService;
            _log = log;
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            _log.LogInformation("Loading recipe with id {RecipeId}", id);

            Recipe = await _repository.GetRecipeDetailAsync(id);

            if (Recipe is null)
            {
                // If id is not for a valid Recipe, generate a 404 error page
                // TODO: Add status code pages middleware to show friendly 404 page
                _log.LogWarning("Could not find recipe with id {RecipeId}", id);
                return NotFound();
            }

            Recipe recipe = await _repository.GetRecipeById(id);
            AuthorizationResult isAuthorized = await _authService.AuthorizeAsync(User, recipe, "CanManageRecipe");
            CanEditRecipe = isAuthorized.Succeeded;

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            await _repository.DeleteRecipeAsync(id);

            return RedirectToPage("/Index");
        }
    }
}