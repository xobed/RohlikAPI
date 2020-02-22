using System;
using System.Collections.Generic;
using System.Net;
using RohlikAPI.Model;

namespace RohlikAPI
{
    public class AuthenticatedRohlikApi : RohlikApi
    {
        public AuthenticatedRohlikApi(string username, string password)
        {
            HttpSessionClient = CreateAuthenticatedHttpClient(username, password);
        }

        protected PersistentSessionHttpClient CreateAuthenticatedHttpClient(string username, string password)
        {
            var loginPostForm = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("do", "loginForm-submit"),
                new KeyValuePair<string, string>("email", username),
                new KeyValuePair<string, string>("password", password)
            };

            const string rohlikLoginUrl = "https://www.rohlik.cz/uzivatel/prihlaseni";

            var httpSessionClient = new PersistentSessionHttpClient();

            var response = httpSessionClient.Post(rohlikLoginUrl, loginPostForm);
            var responseContent = response.Content.ReadAsStringAsync().Result;
            if (responseContent.Contains("Zadal(a) jste nesprávný e-mail nebo heslo"))
            {
                throw new WebException($"Failed to login to Rohlik. Used email: {username}");
            }
            return httpSessionClient;
        }

        public RohlikovacResult RunRohlikovac()
        {
            var rohlikovac = new Rohlikovac(HttpClient);
            return rohlikovac.Run();
        }

        public IEnumerable<Order> GetOrderHistory()
        {
            return GetOrderHistory(DateTime.MinValue);
        }

        public IEnumerable<Order> GetOrderHistory(DateTime getOrdersSince)
        {
            var orderHistory = new OrderHistory(HttpClient);
            return orderHistory.GetOrdersSinceDate(getOrdersSince);
        }
    }
}