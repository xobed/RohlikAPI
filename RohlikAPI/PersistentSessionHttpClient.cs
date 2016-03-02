using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RohlikAPISharp
{
    internal class PersistentSessionHttpClient
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

        public HttpResponseMessage Post(string url, IEnumerable<KeyValuePair<string, string>> parameters)
        {
            HttpContent content = new FormUrlEncodedContent(parameters);
            var response = httpClient.PostAsync(url, content).Result;
            return response;
        }

        public CookieCollection GetCurrentCookies(string domainUrl)
        {
            return cookieContainer.GetCookies(new Uri(domainUrl));
        }
    }
}
