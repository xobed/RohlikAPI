using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RohlikAPI;
using RohlikAPI.Model;

namespace RohlikAPITests
{
    [TestClass]
    public class RohlikApiTests
    {
        [TestMethod]
        public void RohlikApiClient_SelectsCity()
        {
            Assert.IsNotNull(new RohlikApi(City.Brno));
            Assert.IsNotNull(new RohlikApi(City.Praha));
        }

        private void VerifyProduct(Product product, bool isDiscounted)
        {
            Assert.IsNotNull(product.Name, "Product name was null");
            Assert.IsNotNull(product.ProductUrl, $"Product {product.Name} does not have an url");
            Assert.IsTrue(product.Price > 0, $"Product {product.Name} does not have price");

            if (isDiscounted)
            {
                Assert.IsNotNull(product.DiscountedUntil, $"Discounted product {product.Name} does not have discounted until");
                Assert.IsNotNull(product.OriginalPrice, $"Discounted product {product.Name} does not have original price");
            }
            else
            {
                Assert.IsNull(product.DiscountedUntil, $"NonDiscounted product {product.Name} has 'discounted until' {product.DiscountedUntil}. Expected null");
                Assert.IsNull(product.OriginalPrice);
            }
        }

        private void VerifyDiscountedProducts(List<Product> products)
        {
            Assert.IsTrue(products.Any(), "Failed to get any products");
            Assert.IsTrue(products.All(p => p.IsDiscounted), $"Found some products without discount: {string.Join(",", products.Where(p => !p.IsDiscounted).Select(p => p.Name).ToList())}");
            products.ForEach(p => VerifyProduct(p, true));
        }

        private void VerifyNonDiscountedProducts(List<Product> products)
        {
            Assert.IsTrue(products.Any(), "Failed to get any products");
            var nonDicountedProducts = products.Where(p => !p.IsDiscounted).ToList();
            nonDicountedProducts.ForEach(p => VerifyProduct(p, false));
        }

        [TestMethod]
        public void GetMainCategory()
        {
            var api = new RohlikApi(City.Brno);
            var result = api.GetProducts("c300101000-pekarna-a-cukrarna").ToList();
            VerifyNonDiscountedProducts(result);
        }

        [TestMethod]
        public void GetSubcategory()
        {
            var api = new RohlikApi(City.Brno);
            var result = api.GetProducts("c300106206-bio-a-farmarske-konzervovane").ToList();
            VerifyNonDiscountedProducts(result);
        }

        [TestMethod]
        public void GetCenoveTrhaky()
        {
            var api = new RohlikApi(City.Brno);
            var result = api.GetCenoveTrhaky().ToList();
            VerifyDiscountedProducts(result);
        }

        [TestMethod]
        public void GetLastMinute()
        {
            var api = new RohlikApi(City.Brno);
            var result = api.GetLastMinute().ToList();
            VerifyDiscountedProducts(result);
        }
    }
}