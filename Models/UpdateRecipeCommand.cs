using System;
using RecipeApp.Data;

namespace RecipeApp.Models
{
    public class UpdateRecipeCommand : EditRecipeBase
    {
        public Guid Id { get; set; }
        
        public void UpdateRecipe(Recipe recipe)
        {
            recipe.Name = Name;
            recipe.TimeToCook = new TimeSpan(TimeToCookHrs, TimeToCookMins, 0);
            recipe.Method = Method;
            recipe.IsVegetarian = IsVegetarian;
            recipe.IsVegan = IsVegan;
        }
    }
}
