using System.Collections.Generic;
using System.Net;
using System.Web;
using RohlikAPI.Model;

namespace RohlikAPI
{
    public class RohlikApi
    {
        protected PersistentSessionHttpClient HttpSessionClient;
        private readonly City city;

        protected virtual PersistentSessionHttpClient HttpClient => HttpSessionClient ?? CreateHttpClient();

        internal RohlikApi()
        {
        }

        public RohlikApi(City city)
        {
            this.city = city;
        }

        protected virtual PersistentSessionHttpClient CreateHttpClient()
        {
            var setAddressForm = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("do", "addressPopup-form-submit"),
                new KeyValuePair<string, string>("address", city.Address)
            };

            var setCityUrl = "https://www.rohlik.cz/";

            var httpSessionClient = new PersistentSessionHttpClient();

            var response = httpSessionClient.Post(setCityUrl, setAddressForm);
            var responseContent = response.Content.ReadAsStringAsync().Result;
            if (!responseContent.Contains(city.Address))
            {
                throw new WebException($"Failed to set address '{city.Address}' for Rohlik client");
            }
            HttpSessionClient = httpSessionClient;

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
    }
}