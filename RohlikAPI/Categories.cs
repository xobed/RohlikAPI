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
            var rohlikDomain = "https://www.rohlik.cz/";

            var rohlikFrontString = httpClient.Get(rohlikDomain);
            var rohlikFrontDocument = new HtmlDocument();
            rohlikFrontDocument.LoadHtml(rohlikFrontString);

            var categoryNodes = rohlikFrontDocument.DocumentNode.SelectNodes("//div[@data-menu-id='sortiment']//div[contains(@class,'rootcat')]/div/a");
            var categoryHrefs = categoryNodes.Select(c => c.Attributes["href"].Value);
            foreach (var categoryHref in categoryHrefs)
            {
                if (categoryHref.Contains(rohlikDomain))
                {
                    yield return categoryHref.Replace(rohlikDomain, "");
                }
                else
                {
                    yield return categoryHref.Substring(1);
                }
            }
        }
    }
}