using Microsoft.VisualStudio.TestTools.UnitTesting;
using RohlikAPIWeb.Controllers;
using RohlikAPIWeb.Models;

namespace RohlikAPIWebTests.Controllers
{
    [TestClass()]
    public class RohlikSyncTests
    {
        [TestMethod()]
        public void CreateApiResponseTest()
        {
            var sync = new RohlikSync();

            var response = sync.CreateApiResponse();

            Assert.IsNotNull(response.SyncTime);
            Assert.IsNotNull(response.Products);

            foreach (var product in response.Products)
            {
                ValidateProduct(product);
            }
        }

        private void ValidateProduct(ApiProduct product)
        {
            Assert.IsFalse(string.IsNullOrEmpty(product.Name));
            Assert.IsFalse(string.IsNullOrEmpty(product.Href));
            Assert.IsFalse(string.IsNullOrEmpty(product.Sname));
            Assert.IsFalse(string.IsNullOrEmpty(product.Unit));
            Assert.IsFalse(string.IsNullOrEmpty(product.Img));

            Assert.IsTrue(product.Price > 0);
            Assert.IsTrue(product.PPU > 0);
        }
    }
}