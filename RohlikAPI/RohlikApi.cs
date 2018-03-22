using System.Collections.Generic;
using System.Linq;
using System.Net;
using RohlikAPI.Model;

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

        protected PersistentSessionHttpClient CreateHttpClient(string street, string city)
        {
            var setAddressForm = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("do", "addressPopup-form-submit"),
                new KeyValuePair<string, string>("street", street),
                new KeyValuePair<string, string>("city", city)
            };

            var setCityUrl = "https://www.rohlik.cz/";

            var httpSessionClient = new PersistentSessionHttpClient();

            var response = httpSessionClient.Post(setCityUrl, setAddressForm);
            var responseContent = response.Content.ReadAsStringAsync().Result;
            // This is set to non-null if posting address is successful and address is valid for deliveries
            if (responseContent.Contains("\"addressCity\":null"))
            {
                throw new WebException($"Failed to set address '{street} - {city}' for Rohlik client");
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
        ///     Get all products on https://www.rohlik.cz/last-minute
        /// </summary>
        /// <returns>Collection of all 'Last minute' products</returns>
        public IEnumerable<Product> GetLastMinute()
        {
            const string lastMinuteCategory = "last-minute";
            return GetProducts(lastMinuteCategory);
        }

        /// <summary>
        ///     Gets all products on https://www.rohlik.cz/cenove-trhaky
        /// </summary>
        /// <returns>Collection of all 'Cenove trhaky' products</returns>
        public IEnumerable<Product> GetCenoveTrhaky()
        {
            const string cenoveTrhakyCategory = "cenove-trhaky";
            return GetProducts(cenoveTrhakyCategory);
        }

        /// <summary>
        ///     Get URLs for all categories visible on main Rohli.cz page
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