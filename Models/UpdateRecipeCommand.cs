using System;
using RecipeApp.Data;

namespace RecipeApp.Models
{
    public class UpdateRecipeCommand : EditRecipeBase
    {
        public Guid Id { get; set; }
        
        public Recipe UpdateRecipe()
        {
            return new Recipe
            {
                Recipe_Id = Id,
                Name = Name,
                TimeToCook = new TimeSpan(TimeToCookHrs, TimeToCookMins, 0),
                Method = Method,
                IsVegetarian = IsVegetarian,
                IsVegan = IsVegan,
            };
        }
    }
}
