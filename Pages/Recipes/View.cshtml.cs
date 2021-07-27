using System;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeApp.Models;
using RecipeApp.Data;

namespace RecipeApp.Pages.Recipes
{
    public class ViewModel : PageModel
    {
        public RecipeDetailViewModel Recipe { get; set; }
        public bool CanEditRecipe { get; set; }
        private readonly IRecipeRepository _repository;
        private readonly IAuthorizationService _authService;
        public ViewModel(IRecipeRepository repository, IAuthorizationService authService)
        {
            _repository = repository;
            _authService = authService;
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Recipe = await _repository.GetRecipeDetailAsync(id);

            if (Recipe is null)
            {
                // If id is not for a valid Recipe, generate a 404 error page
                // TODO: Add status code pages middleware to show friendly 404 page
                return NotFound();
            }

            Recipe recipe = await _repository.GetRecipeAsync(id);
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