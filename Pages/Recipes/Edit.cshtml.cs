using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using RecipeApp.Data;
using RecipeApp.Models;

namespace RecipeApp.Pages.Recipes
{
    [Authorize]
    public class EditModel : PageModel
    {
        [BindProperty]
        public UpdateRecipeCommand Input { get; set; }
        private readonly IRecipeRepository _repository;
        private readonly IAuthorizationService _authService;
        public EditModel(IRecipeRepository repository, IAuthorizationService authService)
        {
            _repository = repository;
            _authService = authService;
        }

        public async Task<IActionResult> OnGet(Guid id)
        {
            Recipe recipe = await _repository.GetRecipeAsync(id);
            AuthorizationResult authResult = await _authService.AuthorizeAsync(User, recipe, "CanManageRecipe");
            if (!authResult.Succeeded)
            {
                return new ForbidResult();
            }

            Input = await _repository.GetRecipeForUpdateAsync(id);
            if (Input is null)
            {
                // If id is not for a valid Recipe, generate a 404 error page
                // TODO: Add status code pages middleware to show friendly 404 page
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _repository.UpdateRecipeAsync(Input);
                    return RedirectToPage("View", new { id = Input.Id });
                }
            }
            catch (Exception)
            {
                // TODO: Log error
                // Add a model-level error by using an empty string key
                ModelState.AddModelError(
                    string.Empty,
                    "An error occured saving the recipe"
                    );
            }

            //If we got to here, something went wrong
            return Page();
        }
    }
}