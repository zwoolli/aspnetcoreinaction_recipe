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
            Guid id = Guid.NewGuid();
            return new Recipe
            {
                // Perhaps ToRecipe takes an ID and if one isn't provided it creates one for the ID
                Recipe_Id = id,
                Name = Name,
                TimeToCook = new TimeSpan(TimeToCookHrs, TimeToCookMins, 0),
                Method = Method,
                IsVegetarian = IsVegetarian,
                IsVegan = IsVegan,
                Ingredients = Ingredients?.Select(x=>x.ToIngredient(id)).ToList()
            };
        }
    }
}
