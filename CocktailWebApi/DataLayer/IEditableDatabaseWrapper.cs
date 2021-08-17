using CocktailWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailWebApi.DataLayer
{
    /// <summary>
    /// Inteface to abstract a read/write database 
    /// </summary>
    interface IEditableDatabaseWrapper : IDatabaseWrapper
    {
        /// <summary>
        /// Prefix for id items to distinguish between local and web indices 
        /// </summary>
        string IdPrefix { get; }

        /// <summary>
        /// Adds Cocktail to the database
        /// </summary>
        /// <param name="newCocktail"> Cocktail to be added</param>
        /// <returns>The id of the newly added cocktail</returns>
        string AddCocktail(Cocktail newCocktail);

        /// <summary>
        /// Updates existing cocktail in the database
        /// </summary>
        /// <param name="updateCocktail">Cocktail to be updated</param>
        /// <returns>returns the id of the updated cocktail or null if cocktail was not found</returns>
        string UpdateCocktail(Cocktail updateCocktail);

        /// <summary>
        /// Deletes the cocktail with the given id
        /// </summary>
        /// <param name="id">id of cocktail to delete</param>
        /// <returns>The id if cocktail deleted, else null</returns>
        string DeleteCocktail(string id);

    }
}
