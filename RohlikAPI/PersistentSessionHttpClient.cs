using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace RohlikAPI
{
    public class PersistentSessionHttpClient
    {
        private readonly HttpClient httpClient;

        public PersistentSessionHttpClient()
        {
            var cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler {CookieContainer = cookieContainer};
            httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.130 Safari/537.36");
        }

        public string Get(string url)
        {
            var response = httpClient.GetStringAsync(url).Result;
            return response;
        }    

        public T Get<T>(string url)
        {
            var response = httpClient.GetStringAsync(url).Result;
            return JsonConvert.DeserializeObject<T>(response);
        }

        public string Get(HttpRequestMessage requestMessage)
        {
            var result = httpClient.SendAsync(requestMessage).Result.Content.ReadAsStringAsync().Result;
            return result;
        }

        public string Get(HttpRequestMessage requestMessage, params KeyValuePair<string, string>[] urlParameters)
        {
            if (urlParameters == null || urlParameters.Length == 0)
            {
                throw new ArgumentException(nameof(urlParameters));
            }

            var query = HttpUtility.ParseQueryString(string.Empty);
            foreach (var parameter in urlParameters)
            {
                query[parameter.Key] = parameter.Value;
            }

            var urlWithParameters = $"{requestMessage.RequestUri}?{query}";
            requestMessage.RequestUri = new Uri(urlWithParameters);
            return Get(requestMessage);
        }

        public HttpResponseMessage Post(string url, IEnumerable<KeyValuePair<string, string>> parameters)
        {
            HttpContent content = new FormUrlEncodedContent(parameters);
            var response = httpClient.PostAsync(url, content).Result;
            return response;
        }

        public HttpResponseMessage PostJson(string url, object data)
        {
            var serialized = JsonConvert.SerializeObject(data);
            HttpContent content = new StringContent(serialized, Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(url, content).Result;
            return response;
        }
    }
}