using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RecipeApp.Data;
using RecipeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IRecipeRepository _repository;
        private readonly ILogger<IndexModel> _log;
        public IEnumerable<RecipeSummaryViewModel> Recipes { get; private set; }

        public IndexModel(IRecipeRepository repository, ILogger<IndexModel> log)
        {
            _repository = repository;
            _log = log;
        }

        public async Task OnGet()
        {
            Recipes = await _repository.GetRecipesForSummary();
            _log.LogInformation("Loaded {RecipeCount} recipes", Recipes.ToList().Count);
        }
    }
}
