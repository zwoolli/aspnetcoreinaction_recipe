using System;

namespace RecipeApp.Data
{
    public class Ingredient
    {
        public Guid Ingredient_Id { get; set; }
        public Guid Recipe_Id { get; set; }
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
    }
}