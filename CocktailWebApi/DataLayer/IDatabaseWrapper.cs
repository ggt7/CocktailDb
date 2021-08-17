using CocktailWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailWebApi.DataLayer
{
    /// <summary>
    /// Interface to abstract a database that can be read from but not written to
    /// </summary>
    interface IDatabaseWrapper
    {
        /// <summary>
        /// Gets cocktail with the given id
        /// </summary>
        /// <param name="id">The id to search for</param>
        /// <returns>The Cocktail found or null if none found</returns>
        Cocktail GetCocktail(string id);

        /// <summary>
        /// Gets all Cocktails with a name that starts with the given letter
        /// </summary>
        /// <param name="firstLetter">letter to search for at start of Name</param>
        /// <returns> collection of cocktails starting with the given letter</returns>
        IEnumerable<Cocktail> GetCocktailsByFirstLetter(char firstLetter);

        /// <summary>
        /// Get all Cocktails that use the given ingredient
        /// </summary>
        /// <param name="ingredient"> the ingredient to search for</param>
        /// <returns> collection of cocktails that include the ingredient</returns>
        IEnumerable<Cocktail> GetCocktailsByIngredient(string ingredient);

        /// <summary>
        /// Gets all cocktails in the given category
        /// </summary>
        /// <param name="category">the category to search for</param>
        /// <returns>Collection of cocktails in the given category</returns>
        IEnumerable<Cocktail> GetCocktailsByCategory(string category);

        /// <summary>
        /// Get all cocktails using the given glass
        /// </summary>
        /// <param name="glass">the glass to search for</param>
        /// <returns>Collection of cocktails using the given glass</returns>
        IEnumerable<Cocktail> GetCocktailsByGlass(string glass);

        /// <summary>
        /// Get all cocktails meeting the alcohol preference 
        /// </summary>
        /// <param name="alcoholic">given alcohol preference to search for</param>
        /// <returns>Collection of alcoholic or non-alcoholic drinks</returns>
        IEnumerable<Cocktail> GetCocktailsByAlcoholic(string alcoholic);

        /// <summary>
        /// Gets all cocktails with names containing the input string
        /// </summary>
        /// <param name="searchTerm">string to search for in the name</param>
        /// <returns>Collection of cocktails with names including searchterm</returns>
        IEnumerable<Cocktail> SearchCocktails(string searchTerm);

        /// <summary>
        /// Finds all cocktails meeting the filter
        /// </summary>
        /// <param name="filter">input for search term, glass, category, etc to be filtered for</param>
        /// <returns>cocktails matching the filter</returns>
        IEnumerable<Cocktail> GetCocktails(CocktailFilter filter);

    }
}
