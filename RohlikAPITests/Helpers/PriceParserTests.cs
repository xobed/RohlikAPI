using Microsoft.VisualStudio.TestTools.UnitTesting;
using RohlikAPI.Helpers;

namespace RohlikAPITests.Helpers
{
    [TestClass]
    public class PriceParserTests
    {
        [TestMethod]
        public void PriceParser_ParsesCommaPrice()
        {
            var parser = new PriceParser();
            var price = parser.ParsePrice("12,34 Kč");

            Assert.IsTrue(price.Equals(12.34));
        }

        [TestMethod]
        public void PriceParser_ParsesUnitPrice()
        {
            var parser = new PriceParser();
            var price = parser.ParsePrice("1 012,34 Kč/Kg");

            Assert.IsTrue(price.Equals(1012.34));
        }
    }
}