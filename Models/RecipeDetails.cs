using System.Collections.Generic;
using System;

namespace RecipeApp.Models
{
    public class RecipeDetailViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Method { get; set; }

        public DateTimeOffset LastModified { get; set; }
        public IEnumerable<Item> Ingredients { get; set; }

        public class Item
        {
            public string Name { get; set; }
            public string Quantity { get; set; }
        }
    }
}
