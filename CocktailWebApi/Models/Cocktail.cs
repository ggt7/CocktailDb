using System;
using System.Collections.Generic;

namespace CocktailWebApi.Models
{
    public class Cocktail
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public string Category { get; set; }
        public string Glass { get; set; }
        public string Alcoholic { get; set; }
        public string Instructions { get; set; }
        public List<string> Ingredients { get; set; }
        public List<string> Measurements { get; set; }
    }


}
