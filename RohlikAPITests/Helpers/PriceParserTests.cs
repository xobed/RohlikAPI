using Microsoft.VisualStudio.TestTools.UnitTesting;
using RohlikAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RohlikAPI.HelpersTests
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