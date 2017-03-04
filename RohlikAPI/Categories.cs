using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace RohlikAPI
{
    internal class Categories
    {
        private readonly PersistentSessionHttpClient httpClient;

        internal Categories(PersistentSessionHttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public IEnumerable<string> GetAllCategories()
        {
            var rohlikFrontString = httpClient.Get("https://www.rohlik.cz/");
            var rohlikFrontDocument = new HtmlDocument();
            rohlikFrontDocument.LoadHtml(rohlikFrontString);

            var categories = rohlikFrontDocument.DocumentNode.SelectNodes("//div[contains(@id,'rootcat')]/div/a");
            return categories.Select(c => c.Attributes["href"].Value.Substring(1));
        }
    }
}