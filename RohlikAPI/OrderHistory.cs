using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Newtonsoft.Json;
using RohlikAPI.Helpers;
using RohlikAPI.Model;
using RohlikAPI.Model.JsonDeserialization;

namespace RohlikAPI
{
    internal class OrderHistory
    {
        private readonly PersistentSessionHttpClient httpClient;
        
        internal OrderHistory(PersistentSessionHttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        internal OrdersResponse GetOrders()
        {
            var response = httpClient.Get("https://www.rohlik.cz/services/frontend-service/v2/user-profile/orders?limit=999999");
            return JsonConvert.DeserializeObject<OrdersResponse>(response);
        }
    }
}