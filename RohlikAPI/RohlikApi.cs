using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using RohlikAPI.Model;
using RohlikAPI.Model.JsonDeserialization;

namespace RohlikAPI
{
    public class RohlikApi
    {
        protected PersistentSessionHttpClient HttpSessionClient;

        protected virtual PersistentSessionHttpClient HttpClient => HttpSessionClient;

        internal RohlikApi()
        {
        }

        public RohlikApi(City city)
        {
            HttpSessionClient = CreateHttpClient(city.Street, city.CityName);
        }

        public RohlikApi(string street, string city)
        {
            HttpSessionClient = CreateHttpClient(street, city);
        }

        private PersistentSessionHttpClient CreateHttpClient(string street, string city)
        {
            var request = new SetAddressRequest
            {
                StreetWithNumber = street,
                City = city,
                IsGeocodeResult = false
            };

            var homeUrl = "https://www.rohlik.cz/";
            var setCityUrl = "https://www.rohlik.cz/services/frontend-service/delivery-address/check";

            var httpSessionClient = new PersistentSessionHttpClient();
            httpSessionClient.Get(homeUrl);
            var responseString = httpSessionClient.PostJson(setCityUrl, request);
            var response = JsonConvert.DeserializeObject<SetAddressResponse>(responseString.Content.ReadAsStringAsync().Result);

            if (response.Data.Address.City.ToLower() != city.ToLower())
            {
                throw new Exception("Failed to set address / city.");
            }

            return httpSessionClient;
        }

        /// <summary>
        ///     <para>Get all listed Rohlik.cz products</para>
        ///     <para>Works for main categories as well as for sub-categories</para>
        ///     <para>
        ///         Use second part of URL as category string. E.g. https://www.rohlik.cz/c133319-konzervovane-pastiky-a-maso =>
        ///         c133319-konzervovane-pastiky-a-maso
        ///     </para>
        /// </summary>
        /// <param name="category">Product category - e.g. c75455-pecivo or cenove-trhaky</param>
        /// <returns>Collection of products</returns>
        public IEnumerable<Product> GetProducts(string category)
        {
            var products = new Products(HttpClient);
            return products.Get(category);
        }

        public IEnumerable<Product> SearchProducts(string searchString)
        {
            var products = new Products(HttpClient);
            return products.Search(searchString);
        }

        /// <summary>
        ///     Get URLs for all categories visible on main Rohlik.cz page
        /// </summary>
        /// <returns>List of category URL strings</returns>
        public IEnumerable<string> GetCategories()
        {
            var categories = new Categories(HttpClient);
            return categories.GetAllCategories();
        }

        /// <summary>
        ///     Get all products available in all categories
        /// </summary>
        /// <returns>List of all products</returns>
        public IEnumerable<Product> GetAllProducts()
        {
            var categories = GetCategories();
            var allProducts = categories.SelectMany(GetProducts);
            return allProducts.Distinct(new ProductComparer());
        }
    }
}