using CocktailWebApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CocktailWebApi.DataLayer
{
    /// <summary>
    /// Basic local data storage using XML file. Should be replaced with SQL Database
    /// </summary>
    public class LocalXmlDatabase : IEditableDatabaseWrapper
    {
        protected string localXmlFile;
        protected List<Cocktail> cocktailList;
        public string IdPrefix
        { 
            get { return "LOC"; }
        }

        public LocalXmlDatabase(string fileName)
        {
            this.localXmlFile = fileName;
            this.LoadFromFile();
        }

        public Cocktail GetCocktail(string id)
        {
            return cocktailList.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Cocktail> GetCocktails(CocktailFilter filter)
        {
            return this.cocktailList.Where(c =>filter.MatchesFilter(c)).ToList();
        }

        public IEnumerable<Cocktail> GetCocktailsByCategory(string category)
        {
            return cocktailList.Where(x => x.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Cocktail> GetCocktailsByFirstLetter(char firstLetter)
        {
            return cocktailList.Where(x => x.Name.StartsWith(firstLetter.ToString(), StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Cocktail> GetCocktailsByGlass(string glass)
        {
            return cocktailList.Where(x => x.Glass.Equals(glass, StringComparison.OrdinalIgnoreCase));

        }

        public IEnumerable<Cocktail> GetCocktailsByAlcoholic(string alcoholic)
        {
            return cocktailList.Where(x => x.Alcoholic.Equals(alcoholic, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Cocktail> GetCocktailsByIngredient(string ingredient)
        {
            return cocktailList.Where(x => x.Ingredients.Contains(ingredient, StringComparer.OrdinalIgnoreCase));

        }

        public IEnumerable<Cocktail> SearchCocktails(string searchTerm)
        {
            return cocktailList.Where(x => x.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        }

        public string AddCocktail(Cocktail newCocktail)
        {
            do
            {
                Random rand = new Random();
                newCocktail.Id = IdPrefix + rand.Next(1, 10000).ToString();
            }
            while (cocktailList.FirstOrDefault(x => x.Id == newCocktail.Id) != null);
            this.cocktailList.Add(newCocktail);
            this.SaveToFile();
            return newCocktail.Id;
        }
        public string UpdateCocktail(Cocktail updateCocktail)
        {
            int removedCount = cocktailList.RemoveAll(x => x.Id == updateCocktail.Id);
            if (removedCount > 0)
            {
                cocktailList.Add(updateCocktail);
                this.SaveToFile();
            }
            return removedCount > 0 ? updateCocktail.Id : null;
        }

        public string DeleteCocktail(string id)
        {
            int removedCount = cocktailList.RemoveAll(x => x.Id == id);
            this.SaveToFile();
            return removedCount > 0 ? id : null;
        }


        protected void LoadFromFile()
        {
            if (this.localXmlFile == null) return;
            List<Cocktail> readList = null;
            using(FileStream fs = File.OpenRead(this.localXmlFile))
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                XmlSerializer ser = new XmlSerializer(typeof(List<Cocktail>));
                readList = (List<Cocktail>)ser.Deserialize(fs);
            }
            this.cocktailList = readList;
        }

        protected void SaveToFile()
        {
            if (this.localXmlFile == null) return;
            using (FileStream fs = File.Create(this.localXmlFile))
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                XmlSerializer ser = new XmlSerializer(typeof(List<Cocktail>));
                ser.Serialize(fs, this.cocktailList, ns);
            }
        }
    }
}
