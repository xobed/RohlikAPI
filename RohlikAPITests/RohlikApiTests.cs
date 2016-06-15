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

        private void VerifyDiscountedProducts(List<Product> products)
        {
            Assert.IsTrue(products.Any(), "Failed to get any products");
            Assert.IsTrue(products.All(p => p.IsDiscounted), $"Found some products without discount: {string.Join(",",products.Where(p => !p.IsDiscounted).Select(p => p.Name).ToList())}");
            Assert.IsTrue(products.All(p => p.DiscountedUntil != null));
            Assert.IsTrue(products.All(p => p.OriginalPrice != null));
            Assert.IsFalse(products.All(p => string.IsNullOrEmpty(p.ProductUrl)));
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

        private void VerifyNonDiscountedProducts(List<Product> products)
        {
            Assert.IsTrue(products.Any());
            var anyNonDiscountedProduct = products.First(p => !p.IsDiscounted);
            Assert.IsTrue(anyNonDiscountedProduct.DiscountedUntil == null);
            Assert.IsTrue(anyNonDiscountedProduct.OriginalPrice == null);
            Assert.IsTrue(anyNonDiscountedProduct.Price > 0);
            Assert.IsTrue(anyNonDiscountedProduct.Name != null);
            Assert.IsFalse(products.All(p => string.IsNullOrEmpty(anyNonDiscountedProduct.ProductUrl)));
        }

        [TestMethod]
        public void GetCerstvePecivo()
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
    }
}