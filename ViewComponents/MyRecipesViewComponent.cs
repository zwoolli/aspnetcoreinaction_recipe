using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using RecipeApp.Data;
using RecipeApp.Models;
using System;
using System.Collections.Generic;

namespace RecipeApp.ViewComponents
{
    public class MyRecipesViewComponent : ViewComponent
    {
        private readonly IRecipeRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        public MyRecipesViewComponent(IRecipeRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(int numberOfRecipes)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View("Unauthenticated");
            }
            
            string userId = _userManager.GetUserId(HttpContext.User);
            IEnumerable<RecipeSummaryViewModel> recipes = await _repository.GetRecipesByUser(Guid.Parse(userId));

            return View("MyRecipes", recipes.Take(numberOfRecipes));
        }
    }
}