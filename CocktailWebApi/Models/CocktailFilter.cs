using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailWebApi.Models
{
    public class CocktailFilter
    {
        public string SearchItem { get; set; }
        public char? FirstLetter { get; set; }
        public string Category { get; set; }
        public string Glass { get; set; }
        public string Alcoholic { get; set; }
        public IList<string> Ingredients { get; set; }

        public bool MatchesFilter(Cocktail c)
        {
            bool matching =
            (this.SearchItem == null || c.Name.Contains(this.SearchItem, StringComparison.OrdinalIgnoreCase)) &&
            (this.FirstLetter == null || c.Name.StartsWith(this.FirstLetter.ToString(), StringComparison.OrdinalIgnoreCase)) &&
            (this.Category == null || c.Category.Equals(this.Category, StringComparison.OrdinalIgnoreCase)) &&
            (this.Glass == null || c.Glass.Equals(this.Glass,StringComparison.OrdinalIgnoreCase)) &&
            (this.Alcoholic == null || c.Alcoholic.Equals(this.Alcoholic,StringComparison.OrdinalIgnoreCase)) &&
            (this.Ingredients == null || c.Glass == this.Glass);

            if(matching)
            {
                foreach (string filterIngredient in Ingredients)
                    matching &= c.Ingredients.Contains(filterIngredient, StringComparer.OrdinalIgnoreCase);
            }
            return matching;
        }
    }
}
