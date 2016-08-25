using Microsoft.VisualStudio.TestTools.UnitTesting;
using RohlikAPI.Helpers;

namespace RohlikAPITests.Helpers
{
    [TestClass()]
    public class PriceParserTests
    {
        [TestMethod()]
        public void PriceParser_ParsesPrice()
        {
            var parser = new PriceParser();
            var price = parser.ParsePrice("12,34 Kč");

            Assert.IsTrue(price.Equals(12.34), "Failed to parse price");
        }
    }
}