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
            Assert.IsTrue(product.PricePerUnit > 0, $"Product {product.Name} does not have price per unit");
            Assert.IsFalse(string.IsNullOrEmpty(product.Unit), $"Product {product.Name} does not have unit text");
            Assert.IsFalse(string.IsNullOrEmpty(product.ImageUrl), $"Product {product.Name} does not have image url");
            Assert.IsFalse(product.Unit.Contains("(") || product.Unit.Contains(")"), $"Product {product.Name} - unit contains unexpected characters - '{product.Unit}'");

            if (isDiscounted)
            {
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

            // At least some products have discounted until
            Assert.IsTrue(products.Any(p => p.DiscountedUntil != null), "No products with discount expiration found");
            Assert.IsTrue(products.All(p => p.IsDiscounted), $"Found some products without discount: {string.Join(",", products.Where(p => !p.IsDiscounted).Select(p => p.Name).ToList())}");
            products.ForEach(p => VerifyProduct(p, true));
        }

        private void VerifyLastMinuteProducts(List<Product> products)
        {
            Assert.IsTrue(products.Any(), "Failed to get any products");

            // Last minute products do not have 'discounted until'
            Assert.IsTrue(products.All(p => p.DiscountedUntil == null));
            Assert.IsTrue(products.All(p => p.IsDiscounted), $"Found some products without discount: {string.Join(",", products.Where(p => !p.IsDiscounted).Select(p => p.Name).ToList())}");
            products.ForEach(p => VerifyProduct(p, true));
        }

        private void VerifyProductsAllowErrorMargin(List<Product> products, bool discounted)
        {
            var passed = 0;
            foreach (var product in products)
            {
                try
                {
                    VerifyProduct(product, discounted);
                    passed++;
                }
                catch (Exception)
                {
                    // Some assertions may fail - e.g. products without unit, products without price per unit - errors on Rohlik.cz
                }
            }

            // 30% of verified products may fail
            const double allowedErrorMarginPercentage = 0.3;
            var failedPercentage = 1 - (float) passed / products.Count;
            Assert.IsTrue(failedPercentage < allowedErrorMarginPercentage);
        }

        private void VerifyNonDiscountedProducts(List<Product> products)
        {
            Assert.IsTrue(products.Any(), "Failed to get any products");
            var nonDicountedProducts = products.Where(p => !p.IsDiscounted).ToList();
            VerifyProductsAllowErrorMargin(nonDicountedProducts, false);
        }

        [TestMethod]
        public void GetMainCategory()
        {
            const string testCategory = "c300106000-trvanlive";

            var api = new RohlikApi(City.Brno);
            var result = api.GetProducts(testCategory).ToList();
            Assert.IsTrue(result.Any(p => p.IsDiscounted), $"No discounted products found in category {testCategory}. Expected to find at least 1 discounted in non-discounted category");
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
        public void SearchProductsTest()
        {
            var api = new RohlikApi(City.Brno);
            var result = api.SearchProducts("Nestle").ToList();
            VerifyNonDiscountedProducts(result);
        }

        [TestMethod]
        public void GetAllCategories()
        {
            var api = new RohlikApi(City.Brno);
            var results = api.GetCategories().ToList();
            Assert.IsTrue(results.Any());
            foreach (var result in results)
            {
                Assert.IsFalse(result.ToLower().Contains("rohlik.cz"));
                Assert.IsFalse(result.Contains("?"));
            }
        }

        [TestMethod]
        public void GetAllProducts()
        {
            var api = new RohlikApi(City.Brno);
            var allProducts = api.GetAllProducts().ToList();
            VerifyNonDiscountedProducts(allProducts);
        }
    }
}