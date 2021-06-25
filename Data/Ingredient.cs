namespace RecipeApp.Data
{
    public class Ingredient
    {
        public int Ingredient_Id { get; set; }
        public int Recipe_Id { get; set; }
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
    }
}