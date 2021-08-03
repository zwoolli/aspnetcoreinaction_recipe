using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using RecipeApp.Data;

namespace RecipeApp.Models
{
    public class CreateRecipeCommand : EditRecipeBase
    {
        public IList<CreateIngredientCommand> Ingredients { get; set; } = new List<CreateIngredientCommand>();
        public Guid User_Id { get; set; }
        public Recipe ToRecipe([Optional] Guid id)
        {
            if (id == Guid.Empty) id = Guid.NewGuid();

            return new Recipe
            {
                // Perhaps ToRecipe takes an ID and if one isn't provided it creates one for the ID
                Recipe_Id = id,
                User_Id = User_Id,
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
