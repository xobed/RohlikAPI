using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;

namespace RohlikAPI
{
    public class PersistentSessionHttpClient
    {
        private readonly CookieContainer cookieContainer;
        private readonly HttpClient httpClient;

        public PersistentSessionHttpClient()
        {
            cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler { CookieContainer = cookieContainer };
            httpClient = new HttpClient(handler);
        }

        public string Get(string url)
        {
            var response = httpClient.GetStringAsync(url).Result;
            return response;
        }

        public string Get(HttpRequestMessage requestMessage, params KeyValuePair<string,string>[] urlParameters)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            foreach (var parameter in urlParameters)
            {
                query[parameter.Key] = parameter.Value;
            }

            string urlWithParameters = $"{requestMessage.RequestUri}?{query}";
            requestMessage.RequestUri = new Uri(urlWithParameters);
            var result = httpClient.SendAsync(requestMessage).Result.Content.ReadAsStringAsync().Result;
            return result;
        }

        public HttpResponseMessage Post(string url, IEnumerable<KeyValuePair<string, string>> parameters)
        {
            HttpContent content = new FormUrlEncodedContent(parameters);
            var response = httpClient.PostAsync(url, content).Result;
            return response;
        }
    }
}
