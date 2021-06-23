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
        private readonly RecipeStore _service;
        public IEnumerable<RecipeSummaryViewModel> Recipes { get; private set; }

        public IndexModel(RecipeStore service)
        {
            _service = service;
        }

        public async Task OnGet()
        {
            Recipes = await _service.GetRecipes();

        }
    }
}
