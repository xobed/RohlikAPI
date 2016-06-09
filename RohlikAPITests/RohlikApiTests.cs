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
            if (products == null) throw new ArgumentNullException(nameof(products));
            Assert.IsTrue(products.Any());
            Assert.IsTrue(products.All(p => p.IsDiscounted));
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
            var result = api.GetProducts("c75455-pecivo").ToList();
            VerifyNonDiscountedProducts(result);
            
        }

        [TestMethod]
        public void GetSubcategory()
        {
            var api = new RohlikApi(City.Brno);
            var result = api.GetProducts("c133319-konzervovane-pastiky-a-maso").ToList();
            VerifyNonDiscountedProducts(result);
        }
    }
}