using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using RohlikAPI.Model;
using RohlikAPI.Model.JsonDeserialization;

namespace RohlikAPI
{
    internal class Products
    {
        private const string BaseUrl = "https://www.rohlik.cz/";
        private readonly RohlikHttpClient httpClient;

        internal Products(RohlikHttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        internal IEnumerable<Product> Get(string category)
        {
            var categoryId = GetCategoryId(category);
            if (categoryId > 0)
            {
                var rohlikProducts = GetProductsForCategoryViaFrontendService(categoryId);
                var products = GetProductsFromRohlikProducts(rohlikProducts).ToList();
                if (products.Count == 0)
                    throw new RohlikApiException($"Failed to find any products for category {category}");

                return products;
            }

            throw new RohlikApiException($"Invalid category: '{category}'");
        }

        internal IEnumerable<Product> Search(string searchString)
        {
            var rohlikProducts = SearchProductsViaFrontendService(searchString);
            return GetProductsFromRohlikProducts(rohlikProducts).ToList();
        }

        private Product GetProductFromRohlikProduct(RohlikProduct rohlikProduct)
        {
            var product = new Product
            {
                Name = rohlikProduct.ProductName,
                Price = rohlikProduct.Price.Full,
                PricePerUnit = rohlikProduct.PricePerUnit.Full,
                ProductUrl = $"{BaseUrl}{rohlikProduct.BaseLink}",
                IsSoldOut = rohlikProduct.InStock == false,
                ImageUrl = $"https://www.rohlik.cz/cdn-cgi/image/f=auto,w=300,h=300/https://cdn.rohlik.cz/{rohlikProduct.ImgPath}",
                Unit = rohlikProduct.Unit
            };
            var nonExpirationSales = rohlikProduct.Sales.FirstOrDefault(s => s.Type == "sale");
            product.IsDiscounted = rohlikProduct.GoodPrice | (nonExpirationSales != null);

            if (nonExpirationSales != null)
            {
                product.DiscountedUntil = nonExpirationSales.EndsAtDateTime;
                product.OriginalPrice = nonExpirationSales.OriginalPrice.Full;
            }

            return product;
        }

        private IEnumerable<Product> GetProductsFromRohlikProducts(IEnumerable<RohlikProduct> productList)
        {
            return productList.Select(GetProductFromRohlikProduct);
        }

        private long GetCategoryId(string category)
        {
            var digitString = new string(category.Where(char.IsDigit).ToArray());
            if (digitString.Length == 0) return 0;

            var categoryId = long.Parse(digitString);
            return categoryId;
        }

        private List<RohlikProduct> GetProductsForCategoryViaFrontendService(long categoryId)
        {
            var url = $"{BaseUrl}services/frontend-service/products/{categoryId}?offset=0&limit=100000";
            var response = httpClient.Get<ProductResponseJson>(url);
            return response.Data.ProductList;
        }

        private List<RohlikProduct> SearchProductsViaFrontendService(string query)
        {
            var url = $"{BaseUrl}services/frontend-service/search/{query}?&offset=0&limit=100000";
            var response = httpClient.Get<ProductResponseJson>(url);
            return response.Data.ProductList;
        }
    }
}