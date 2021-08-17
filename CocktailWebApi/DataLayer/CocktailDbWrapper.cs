using CocktailWebApi.DataLayer;
using CocktailWebApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace CocktailWebApi.Data
{
    public class CocktailDbWrapper : IDatabaseWrapper
    {

        const string baseURL = "http://www.thecocktaildb.com/api/json/v1/1/";
        const string lookUpById = baseURL + "lookup.php?i=";
        const string searchCocktails = baseURL + "search.php?s=";
        const string listByFirstLetter = baseURL + "search.php?f=";
        const string listByIngredient = baseURL + "filter.php?i=";
        const string listByCategory = baseURL + "filter.php?c=";
        const string listByGlass = baseURL + "filter.php?g=";
        const string listByAlcohol = baseURL + "filter.php?a=";

        private static readonly HttpClient client = new HttpClient();

        public Cocktail GetCocktail(string id)
        {
            return GetOneFromWeb(lookUpById + id);
        }

        public IEnumerable<Cocktail> GetCocktailsByFirstLetter(char firstLetter)
        {
            return GetFromWeb(listByFirstLetter + firstLetter);
             
        }
        public IEnumerable<Cocktail> GetCocktailsByIngredient(string ingredient)
        {
            return GetFromWeb(listByIngredient + ingredient);

        }
        public IEnumerable<Cocktail> GetCocktailsByCategory(string cat)
        {
            return GetFromWeb(listByCategory + cat);

        }
        public IEnumerable<Cocktail> GetCocktailsByGlass(string glass)
        {
            return GetFromWeb(listByGlass + glass);

        }
        public IEnumerable<Cocktail> GetCocktailsByAlcoholic(string alcoholic)
        {
            return GetFromWeb(listByAlcohol + alcoholic);

        }
        public IEnumerable<Cocktail> SearchCocktails(string searchTerm)
        {
            return GetFromWeb(searchCocktails + searchTerm);

        }

        public IEnumerable<Cocktail> GetCocktails(CocktailFilter filter)
        {
            List<IEnumerable<Cocktail>> subsets = new List<IEnumerable<Cocktail>>();
            IEnumerable<Cocktail> resultList = null;

            if(filter.SearchItem != null)
                subsets.Add(this.SearchCocktails(filter.SearchItem));

            if (filter.FirstLetter.HasValue)
                subsets.Add(this.GetCocktailsByFirstLetter(filter.FirstLetter.Value));

            if (filter.Category != null) 
                subsets.Add(this.GetCocktailsByCategory(filter.Category));

            if (filter.Glass != null) 
                subsets.Add(this.GetCocktailsByGlass(filter.Glass));

            if (filter.Alcoholic != null) 
                subsets.Add(this.GetCocktailsByAlcoholic(filter.Alcoholic));

            if (filter.Ingredients != null)
            {
                foreach(string i in filter.Ingredients)
                {
                    subsets.Add(this.GetCocktailsByIngredient(i));
                }
            }

            if (subsets.Count > 0)
            {
                resultList = subsets[0];
                for(int i =1; i < subsets.Count; i++)
                {
                    resultList = resultList.Join(subsets[i],
                        sub => sub.Id,
                        res => res.Id,
                        (s, r) => s ).ToList();
                }
            }

            return resultList;
        }

        private static List<Cocktail> GetFromWeb(string uri)
        {
            var response = client.GetAsync(uri).Result;
            var json = response.Content.ReadAsStringAsync().Result;
            CocktailDbResult drinkResult = Newtonsoft.Json.JsonConvert.DeserializeObject<CocktailDbResult>(json);

            List<Cocktail> cocktails;
            if (drinkResult == null || drinkResult.drinks == null)
            {
                cocktails = new List<Cocktail>();
            }
            else
            {
                cocktails = new List<Cocktail>(drinkResult.drinks.Count);

                foreach (CocktailDbItem cdbi in drinkResult.drinks)
                {
                    cocktails.Add(ConvertModel(cdbi));
                }
            }
            return cocktails;
        }

        private static Cocktail GetOneFromWeb(string uri)
        {
            var response = client.GetAsync(uri).Result;
            var json = response.Content.ReadAsStringAsync().Result;
            CocktailDbResult drinkResult = Newtonsoft.Json.JsonConvert.DeserializeObject<CocktailDbResult>(json);
           
            if(drinkResult == null || drinkResult.drinks == null || drinkResult.drinks.Count < 1) 
            {
                return null;
            }
            return ConvertModel(drinkResult.drinks[0]);
        }

        private static Cocktail ConvertModel(CocktailDbItem dbItem)
        {
            if (dbItem == null) return null;
            List<string> ingreds = new List<string>();
            AddIfNotNull<string>(ingreds, dbItem.strIngredient1);
            AddIfNotNull<string>(ingreds, dbItem.strIngredient2);
            AddIfNotNull<string>(ingreds, dbItem.strIngredient3);
            AddIfNotNull<string>(ingreds, dbItem.strIngredient4);
            AddIfNotNull<string>(ingreds, dbItem.strIngredient5);
            AddIfNotNull<string>(ingreds, dbItem.strIngredient6);
            AddIfNotNull<string>(ingreds, dbItem.strIngredient7);
            AddIfNotNull<string>(ingreds, dbItem.strIngredient8);
            AddIfNotNull<string>(ingreds, dbItem.strIngredient9);
            AddIfNotNull<string>(ingreds, dbItem.strIngredient10);
            AddIfNotNull<string>(ingreds, dbItem.strIngredient11);
            AddIfNotNull<string>(ingreds, dbItem.strIngredient12);
            AddIfNotNull<string>(ingreds, dbItem.strIngredient13);
            AddIfNotNull<string>(ingreds, dbItem.strIngredient14);
            AddIfNotNull<string>(ingreds, dbItem.strIngredient15);

            List<string> measure = new List<string>();
            AddIfNotNull<string>(measure, dbItem.strMeasure1);
            AddIfNotNull<string>(measure, dbItem.strMeasure2);
            AddIfNotNull<string>(measure, dbItem.strMeasure3);
            AddIfNotNull<string>(measure, dbItem.strMeasure4);
            AddIfNotNull<string>(measure, dbItem.strMeasure5);
            AddIfNotNull<string>(measure, dbItem.strMeasure6);
            AddIfNotNull<string>(measure, dbItem.strMeasure7);
            AddIfNotNull<string>(measure, dbItem.strMeasure8);
            AddIfNotNull<string>(measure, dbItem.strMeasure9);
            AddIfNotNull<string>(measure, dbItem.strMeasure10);
            AddIfNotNull<string>(measure, dbItem.strMeasure11);
            AddIfNotNull<string>(measure, dbItem.strMeasure12);
            AddIfNotNull<string>(measure, dbItem.strMeasure13);
            AddIfNotNull<string>(measure, dbItem.strMeasure14);
            AddIfNotNull<string>(measure, dbItem.strMeasure15);


            return new Cocktail() {
                Id = dbItem.idDrink,
                Name = dbItem.strDrink,
                Category = dbItem.strCategory,
                Glass = dbItem.strGlass,
                Instructions = dbItem.strInstructions,
                Alcoholic = dbItem.strAlcoholic,
                Ingredients = ingreds,
                Measurements = measure,
            };
        }

        private static void AddIfNotNull<T>(List<T> list, T obj)
        {
            if(obj != null)
            {
                list.Add(obj);
            }
        }

        #region Db Models
        private class CocktailDbResult
        {
            public IList<CocktailDbItem> drinks { get; set; }

            public CocktailDbResult()
            {
                drinks = new List<CocktailDbItem>();
            }
        }

        private class CocktailDbItem
        {

            public string idDrink { get; set; }
            public string strDrink { get; set; }
            public string strCategory { get; set; }
            public string strGlass { get; set; }
            public string strAlcoholic { get; set; }
            public string strInstructions { get; set; }

            #region Ingredients
            public string strIngredient1 { get; set; }
            public string strIngredient2 { get; set; }
            public string strIngredient3 { get; set; }
            public string strIngredient4 { get; set; }
            public string strIngredient5 { get; set; }
            public string strIngredient6 { get; set; }
            public string strIngredient7 { get; set; }
            public string strIngredient8 { get; set; }
            public string strIngredient9 { get; set; }
            public string strIngredient10 { get; set; }
            public string strIngredient11 { get; set; }
            public string strIngredient12 { get; set; }
            public string strIngredient13 { get; set; }
            public string strIngredient14 { get; set; }
            public string strIngredient15 { get; set; }
            #endregion

            #region Measurements
            public string strMeasure1 { get; set; }
            public string strMeasure2 { get; set; }
            public string strMeasure3 { get; set; }
            public string strMeasure4 { get; set; }
            public string strMeasure5 { get; set; }
            public string strMeasure6 { get; set; }
            public string strMeasure7 { get; set; }
            public string strMeasure8 { get; set; }
            public string strMeasure9 { get; set; }
            public string strMeasure10 { get; set; }
            public string strMeasure11 { get; set; }
            public string strMeasure12 { get; set; }
            public string strMeasure13 { get; set; }
            public string strMeasure14 { get; set; }
            public string strMeasure15 { get; set; }
            #endregion
        }
        #endregion
    }
}
