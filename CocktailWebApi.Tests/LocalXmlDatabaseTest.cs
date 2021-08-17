using CocktailWebApi.Data;
using CocktailWebApi.DataLayer;
using CocktailWebApi.Models;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.Collections.Generic;
using System.Linq;

namespace CocktailWebApi.Tests
{
    public class LocalXmlDatabaseTest
    {
        #region MockLocalDb
        private class MockLocalDb : LocalXmlDatabase
        {
            public MockLocalDb() : base("../../../localStaticDb.xml")
            {
            }
            protected new void SaveToFile(){ }
        }
        #endregion


            private LocalXmlDatabase localDb;
        [SetUp]
        public void Setup()
        {
            localDb = new MockLocalDb();
        }

        [TestCase("LOC1")]
        [TestCase("LOC2")]
        [TestCase("LOC3")]
        [TestCase("LOC4")]
        [TestCase("LOC5")]
        [TestCase("LOC1337")]
        public void Get_ValidId(string inputId)
        {
            Cocktail result = localDb.GetCocktail(inputId);
            Assert.AreEqual(inputId, result.Id);
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("1712342495")]
        [TestCase("I'm An ID")]
        public void Get_BadId(string inputId)
        {
            Cocktail result = localDb.GetCocktail(inputId);
            Assert.IsNull(result);
        }

        [TestCase("Glass")]
        [TestCase("Mug")]
        public void GetByGlass_GoodGlass(string glass)
        {
            var result = localDb.GetCocktailsByGlass(glass);
            foreach (Cocktail partial in result)
            {
                Cocktail c = localDb.GetCocktail(partial.Id);
                Assert.AreEqual(glass.ToLower(), c.Glass.ToLower());
            }
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("Colander")]
        public void GetByGlass_BadGlass(string glass)
        {
            var result = localDb.GetCocktailsByGlass(glass);

            Assert.AreEqual(0, result.Count<Cocktail>());
        }

        [TestCase('p')]
        [TestCase('P')]
        [TestCase('s')]
        [TestCase('1')]
        public void GetByFirstLetter_Good(char letter)
        {
            var result = localDb.GetCocktailsByFirstLetter(letter);
            foreach (Cocktail c in result)
            {
                Assert.IsTrue(c.Name.StartsWith(letter.ToString(), System.StringComparison.OrdinalIgnoreCase));
            }
        }

        [TestCase(':')]
        [TestCase('\n')]
        public void GetByFirstLetter_Bad(char letter)
        {
            var result = localDb.GetCocktailsByFirstLetter(letter);

            Assert.AreEqual(0, result.Count<Cocktail>());
        }

        [TestCase("LOC1")]
        [TestCase("LOC2")]
        [TestCase("LOC3")]
        [TestCase("LOC4")]
        [TestCase("LOC5")]
        [TestCase("LOC1337")]
        public void Filter_OnlyOne(string id)
        {
            Cocktail inputCocktail = localDb.GetCocktail(id);
            CocktailFilter filter = new CocktailFilter()
            {
                SearchItem = inputCocktail.Name,
                FirstLetter = inputCocktail.Name[0],
                Glass = inputCocktail.Glass,
                Category = inputCocktail.Category,
                Alcoholic = inputCocktail.Alcoholic,
                Ingredients = inputCocktail.Ingredients,
            };
            var cocktails = localDb.GetCocktails(filter);
            Cocktail filteredCocktail = cocktails.First();

            Assert.AreEqual(inputCocktail.Name, filteredCocktail.Name);
            Assert.AreEqual(inputCocktail.Category, filteredCocktail.Category);
            Assert.AreEqual(inputCocktail.Glass, filteredCocktail.Glass);
            Assert.AreEqual(inputCocktail.Alcoholic, filteredCocktail.Alcoholic);
        }

        [TestCase("LOC1")]
        [TestCase("LOC2")]
        [TestCase("LOC3")]
        [TestCase("LOC4")]
        [TestCase("LOC5")]
        [TestCase("LOC1337")]
        public void Filter_Search_None(string id)
        {
            Cocktail inputCocktail = localDb.GetCocktail(id);
            CocktailFilter filter = new CocktailFilter()
            {
                SearchItem = inputCocktail.Name + "Bad Search",
                FirstLetter = inputCocktail.Name[0],
                Glass = inputCocktail.Glass,
                Category = inputCocktail.Category,
                Alcoholic = inputCocktail.Alcoholic,
                Ingredients = inputCocktail.Ingredients,
            };
            var cocktails = localDb.GetCocktails(filter);
            Assert.AreEqual(0, cocktails.Count<Cocktail>());
        }

        [TestCase("LOC1")]
        [TestCase("LOC2")]
        [TestCase("LOC3")]
        [TestCase("LOC4")]
        [TestCase("LOC5")]
        [TestCase("LOC1337")]
        public void Filter_First_None(string id)
        {
            Cocktail inputCocktail = localDb.GetCocktail(id);
            CocktailFilter filter = new CocktailFilter()
            {
                FirstLetter = (char)(inputCocktail.Name[0] + 1),
                SearchItem = inputCocktail.Name,
                Glass = inputCocktail.Glass,
                Category = inputCocktail.Category,
                Alcoholic = inputCocktail.Alcoholic,
                Ingredients = inputCocktail.Ingredients,
            };
            var cocktails = localDb.GetCocktails(filter);
            Assert.AreEqual(0, cocktails.Count<Cocktail>());
        }

        [TestCase("LOC1")]
        [TestCase("LOC2")]
        [TestCase("LOC3")]
        [TestCase("LOC4")]
        [TestCase("LOC5")]
        [TestCase("LOC1337")]
        public void Filter_Glass_None(string id)
        {
            Cocktail inputCocktail = localDb.GetCocktail(id);
            CocktailFilter filter = new CocktailFilter()
            {
                Glass = inputCocktail.Glass + "Glass",
                SearchItem = inputCocktail.Name,
                FirstLetter = inputCocktail.Name[0],
                Category = inputCocktail.Category,
                Alcoholic = inputCocktail.Alcoholic,
                Ingredients = inputCocktail.Ingredients,
            };
            var cocktails = localDb.GetCocktails(filter);
            Assert.AreEqual(0, cocktails.Count<Cocktail>());

        }

        [TestCase("LOC1")]
        [TestCase("LOC2")]
        [TestCase("LOC3")]
        [TestCase("LOC4")]
        [TestCase("LOC5")]
        [TestCase("LOC1337")]
        public void Filter_Category_None(string id)
        {
            Cocktail inputCocktail = localDb.GetCocktail(id);
            CocktailFilter filter = new CocktailFilter()
            {
                Category = inputCocktail.Category + "Cat",
                SearchItem = inputCocktail.Name,
                FirstLetter = inputCocktail.Name[0],
                Glass = inputCocktail.Glass,
                Alcoholic = inputCocktail.Alcoholic,
                Ingredients = inputCocktail.Ingredients,
            };
            var cocktails = localDb.GetCocktails(filter);
            Assert.AreEqual(0, cocktails.Count<Cocktail>());
        }

        [TestCase("LOC1")]
        [TestCase("LOC2")]
        [TestCase("LOC3")]
        [TestCase("LOC4")]
        [TestCase("LOC5")]
        [TestCase("LOC1337")]
        public void Filter_Alcoholic_None(string id)
        {
            Cocktail inputCocktail = localDb.GetCocktail(id);
            CocktailFilter filter = new CocktailFilter()
            {
                Alcoholic = inputCocktail.Alcoholic + "Not",
                SearchItem = inputCocktail.Name,
                FirstLetter = inputCocktail.Name[0],
                Glass = inputCocktail.Glass,
                Category = inputCocktail.Category,
                Ingredients = inputCocktail.Ingredients,
            };
            var cocktails = localDb.GetCocktails(filter);
            Assert.AreEqual(0, cocktails.Count<Cocktail>());

        }

        [TestCase("LOC1")]
        [TestCase("LOC2")]
        [TestCase("LOC3")]
        [TestCase("LOC4")]
        [TestCase("LOC5")]
        [TestCase("LOC1337")]
        public void Filter_Ingredients_None(string id)
        {
            Cocktail inputCocktail = localDb.GetCocktail(id);

            List<string> badIngreds = new List<string>()
            {
                inputCocktail.Ingredients[0],
                "Arsenic"
            };

            CocktailFilter filter = new CocktailFilter()
            {
                Ingredients = badIngreds,
                SearchItem = inputCocktail.Name,
                FirstLetter = inputCocktail.Name[0],
                Glass = inputCocktail.Glass,
                Category = inputCocktail.Category,
                Alcoholic = inputCocktail.Alcoholic,
            };
            var cocktails = localDb.GetCocktails(filter);
            Assert.AreEqual(0, cocktails.Count<Cocktail>());

        }
    }
}