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

        private RohlikHttpClient CreateAuthenticatedHttpClient(string username, string password)
        {
            const string rohlikLoginUrl = "https://www.rohlik.cz/services/frontend-service/login";

            var httpSessionClient = new RohlikHttpClient();

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
            return HttpClient.Get<RohlikovacResult>("https://www.rohlik.cz/services/frontend-service/credit-forge-roll");
        }

        public IEnumerable<Order> GetOrderHistory()
        {
            var response = HttpClient.Get<OrdersResponse>("https://www.rohlik.cz/services/frontend-service/v2/user-profile/orders?limit=999999");
            return response.Data.Orders;
        }
    }
}