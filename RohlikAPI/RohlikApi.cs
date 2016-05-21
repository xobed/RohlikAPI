using System.Collections.Generic;
using System.Net;

namespace RohlikAPI
{
    public class RohlikApi
    {
        private PersistentSessionHttpClient httpClient;

        internal RohlikApi()
        {
        }

        public RohlikApi(City city)
        {
            httpClient = SetCity(city);
        }

        internal void SetHttpClient(PersistentSessionHttpClient httpClientToSet)
        {
            httpClient = httpClientToSet;
        }

        private PersistentSessionHttpClient SetCity(City city)
        {
            var loginPostForm = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("do", "addressPopup-form-submit"),
                new KeyValuePair<string, string>("address", city.Address),
            };

            string setCityUrl = "https://www.rohlik.cz/";

            var httpSessionClient = new PersistentSessionHttpClient();

            var response = httpSessionClient.Post(setCityUrl, loginPostForm);
            var responseContent = response.Content.ReadAsStringAsync().Result;
            if (!responseContent.Contains(city.Address))
            {
                throw new WebException($"Failed to set address '{city.Address}' for Rohlik client");
            }
            return httpSessionClient;
        }
    }
}