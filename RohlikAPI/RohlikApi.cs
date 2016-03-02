using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RohlikAPI
{
    public class RohlikApi
    {
        private readonly PersistentSessionHttpClient httpClient;

        public RohlikApi(string username, string password)
        {
            httpClient = Login(username, password);
        }

        private PersistentSessionHttpClient Login(string username, string password)
        {
            var loginPostForm = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("do", "loginForm-submit"),
                new KeyValuePair<string, string>("email", username),
                new KeyValuePair<string, string>("password", password)
            };

            const string rohlikLoginUrl = "https://www.rohlik.cz/uzivatel/prihlaseni";

            PersistentSessionHttpClient httpSessionClient = new PersistentSessionHttpClient();

            var response = httpSessionClient.Post(rohlikLoginUrl, loginPostForm);
            var responseContent = response.Content.ReadAsStringAsync().Result;
            if (!responseContent.Contains("Můj účet"))
            {
                throw new WebException($"Failed to login to Rohlik. Used email: {username}");
            }

            return httpSessionClient;
        }

        public string RunRohlikovac()
        {
            Rohlikovac rohlikovac = new Rohlikovac(httpClient);
            return rohlikovac.Run();
        }
    }
}
