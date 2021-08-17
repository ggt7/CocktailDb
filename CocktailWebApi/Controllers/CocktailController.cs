using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CocktailWebApi.Data;
using CocktailWebApi.DataLayer;
using CocktailWebApi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CocktailWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CocktailController : ControllerBase
    {
        //ToDo replace this with services from constructor
        private static IDatabaseWrapper webCocktailDb = new CocktailDbWrapper();
        private static  IEditableDatabaseWrapper localDb = new LocalXmlDatabase("localCocktailDb.xml");
        private readonly ILogger<CocktailController> _logger;

        public CocktailController(ILogger<CocktailController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("{id}")]
        public Cocktail Get(string id)
        {
            Cocktail cocktail = null;
            if (id.StartsWith(localDb.IdPrefix))
            {
                cocktail = localDb.GetCocktail(id);
            }
            else
            {
                cocktail = webCocktailDb.GetCocktail(id);
            }
            return cocktail;
        }

        [HttpPost]
        [Route("filter")]
        public IEnumerable<Cocktail> Get(CocktailFilter filter)
        {
            List<Cocktail> cocktails = webCocktailDb.GetCocktails(filter).ToList();
            cocktails.AddRange(localDb.GetCocktails(filter));
            return cocktails;
        }

        [HttpGet]
        [Route("search/{searchTerm}")]
        public IEnumerable<Cocktail> SearchCocktails(string searchTerm)
        {
            List<Cocktail> cocktails = webCocktailDb.SearchCocktails(searchTerm).ToList();
            cocktails.AddRange(localDb.SearchCocktails(searchTerm));
            return cocktails;
        }
        [HttpGet]
        [Route("firstletter/{letter}")]
        public IEnumerable<Cocktail> GetByFirstLetter(char letter)
        {
            List<Cocktail> cocktails = webCocktailDb.GetCocktailsByFirstLetter(letter).ToList();
            cocktails.AddRange(localDb.GetCocktailsByFirstLetter(letter));
            return cocktails;
        }

        [HttpGet]
        [Route("ingredient/{ingredient}")]
        public IEnumerable<Cocktail> GetByIngredient(string ingredient)
        {
            List<Cocktail> cocktails = webCocktailDb.GetCocktailsByIngredient(ingredient).ToList();
            cocktails.AddRange(localDb.GetCocktailsByIngredient(ingredient));

            return cocktails;

        }

        [HttpGet]
        [Route("glass/{glass}")]
        public IEnumerable<Cocktail> GetByGlass(string glass)
        {
            List<Cocktail> cocktails = webCocktailDb.GetCocktailsByGlass(glass).ToList();
            cocktails.AddRange(localDb.GetCocktailsByGlass(glass));
            return cocktails;
        }

        [HttpGet]
        [Route("category/{category}")]
        public IEnumerable<Cocktail> GetByCategory(string category)
        {
            List<Cocktail> cocktails = webCocktailDb.GetCocktailsByCategory(category).ToList();
            cocktails.AddRange(localDb.GetCocktailsByCategory(category));
            return cocktails;
        }

        [HttpPut]
        [Route("add")]
        public string AddCocktail(Cocktail cocktail)
        {
            return localDb.AddCocktail(cocktail);
        }

        [HttpPut]
        [Route("update")]
        public string UpdateCocktail(Cocktail cocktail)
        {
            return localDb.UpdateCocktail(cocktail);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public string DeleteCocktail(string id)
        {
            return localDb.DeleteCocktail(id);
        }
    }
}
