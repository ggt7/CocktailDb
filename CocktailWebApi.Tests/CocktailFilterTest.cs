using NUnit.Framework;

namespace CocktailWebApi.Tests
{
    public class CocktailFilterTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("", "", "", "")]
        [Ignore("To be Implemented")]
        public void Matches(string name, string category, string glass, string alcoholic, string[] ingredients)
        {
            Assert.Pass();
        }

        public void DoesNotMatch()
        {

        }
    }
}