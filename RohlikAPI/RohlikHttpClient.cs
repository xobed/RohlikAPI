using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace RohlikAPI
{
    public class RohlikHttpClient
    {
        private readonly HttpClient httpClient;

        public RohlikHttpClient()
        {
            httpClient = new HttpClient();
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

        public HttpResponseMessage PostJson(string url, object data)
        {
            var serialized = JsonConvert.SerializeObject(data);
            HttpContent content = new StringContent(serialized, Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(url, content).Result;
            return response;
        }
    }
}