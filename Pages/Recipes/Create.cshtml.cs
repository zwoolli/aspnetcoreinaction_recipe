using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeApp.Models;
using RecipeApp.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace RecipeApp.Pages.Recipes
{
    [Authorize]
    public class CreateModel : PageModel
    {
        [BindProperty]
        public CreateRecipeCommand Input { get; set; }
        private readonly IRecipeRepository _repository;

        public CreateModel(IRecipeRepository repository)
        {
            _repository = repository;
        }
        
        public void OnGet()
        {
            Input = new CreateRecipeCommand();
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Input.User_Id = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                    var id = await _repository.CreateRecipeAsync(Input);
                    return RedirectToPage("View", new { id = id });
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