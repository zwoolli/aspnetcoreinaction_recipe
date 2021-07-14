using System;
using System.Collections.Generic;
using System.Linq;
using RecipeApp.Data;

namespace RecipeApp.Models
{
    public class CreateRecipeCommand : EditRecipeBase
    {
        public IList<CreateIngredientCommand> Ingredients { get; set; } = new List<CreateIngredientCommand>();

        public Recipe ToRecipe()
        {
            return new Recipe
            {
                Recipe_Id = Guid.NewGuid(),
                Name = Name,
                TimeToCook = new TimeSpan(TimeToCookHrs, TimeToCookMins, 0),
                Method = Method,
                IsVegetarian = IsVegetarian,
                IsVegan = IsVegan,
                Ingredients = Ingredients?.Select(x=>x.ToIngredient()).ToList()
            };
        }
    }
}
