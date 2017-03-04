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

            var categoryNodes = rohlikFrontDocument.DocumentNode.SelectNodes("//div[contains(@id,'rootcat')]/div/a");
            var categoryHrefs = categoryNodes.Select(c => c.Attributes["href"].Value);
            return categoryHrefs.Select(c => c.Substring(1));
        }
    }
}