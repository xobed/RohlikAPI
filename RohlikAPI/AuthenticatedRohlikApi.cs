using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using RohlikAPI.Model;
using RohlikAPI.Model.JsonDeserialization;

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
            const string rohlikLoginUrl = "https://www.rohlik.cz/services/frontend-service/login";

            var httpSessionClient = new PersistentSessionHttpClient();

            var response = httpSessionClient.PostJson(rohlikLoginUrl, new Login {Email = username, Password = password});
            var responseContent = response.Content.ReadAsStringAsync().Result;
            var deserialized = JsonConvert.DeserializeObject<LoginResponse>(responseContent);

            if (deserialized.Status != 200)
            {
                var messagesString = string.Join("\n", deserialized.Messages.Select(m => m.Content));
                throw new WebException($"Failed to login: {messagesString}");
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