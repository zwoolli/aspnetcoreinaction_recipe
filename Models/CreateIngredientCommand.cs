using System;
using System.ComponentModel.DataAnnotations;
using RecipeApp.Data;

namespace RecipeApp.Models
{
    public class CreateIngredientCommand
    {
        [Required, StringLength(100)]
        public string Name { get; set; }
        [Range(0, int.MaxValue)]
        public decimal Quantity { get; set; }
        [StringLength(20)]
        public string Unit { get; set; }

        public Ingredient ToIngredient()
        {
            return new Ingredient
            {
                Ingredient_Id = Guid.NewGuid(),
                Name = Name,
                Quantity = Quantity,
                Unit = Unit,
            };
        }
    }
}
