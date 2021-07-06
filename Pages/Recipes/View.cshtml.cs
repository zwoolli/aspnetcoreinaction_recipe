using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IRecipeRepository _repository;
        public ViewModel(IRecipeRepository repository)
        {
            _repository = repository;
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
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            await _repository.DeleteRecipeAsync(id);

            return RedirectToPage("/Index");
        }
    }
}