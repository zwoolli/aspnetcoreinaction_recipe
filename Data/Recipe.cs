using System;
using System.Collections.Generic;

namespace RecipeApp.Data
{
    public class Recipe
    {
        public Guid Recipe_Id { get; set; }
        public Guid User_Id { get; set; }
        public string Name { get; set; }
        public TimeSpan TimeToCook { get; set; }
        public string Method { get; set; }
        public bool IsVegetarian { get; set; }
        public bool IsVegan { get; set; }
        public DateTimeOffset LastModified { get; set; }
        public List<Ingredient> Ingredients { get; set; }
    }
}