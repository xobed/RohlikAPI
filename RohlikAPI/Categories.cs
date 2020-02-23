using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace RohlikAPI
{
    internal class Categories
    {
        private readonly RohlikHttpClient httpClient;

        internal Categories(RohlikHttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public IEnumerable<string> GetAllCategories()
        {
            var rohlikDomain = "https://www.rohlik.cz/";

            var rohlikFrontString = httpClient.Get(rohlikDomain);
            var rohlikFrontDocument = new HtmlDocument();
            rohlikFrontDocument.LoadHtml(rohlikFrontString);

            var categoryNodes = rohlikFrontDocument.DocumentNode.SelectNodes("//li[@class='sortimentItem']//a");
            if (categoryNodes == null)
            {
                throw new Exception("Failed to find category nodes in rohlik main page");
            }
            var categoryHrefs = categoryNodes.Select(c => c.Attributes["href"].Value);
            foreach (var categoryHref in categoryHrefs)
            {
                // strip any parameters
                string hrefWithoutParameters = StripUrlParameters(categoryHref);

                if (hrefWithoutParameters.Contains(rohlikDomain))
                {
                    yield return hrefWithoutParameters.Replace(rohlikDomain, "");
                }
                else
                {
                    yield return hrefWithoutParameters.Substring(1);
                }
            }
        }

        private string StripUrlParameters(string categoryHref)
        {
            var questionMarkIndex = categoryHref.IndexOf("?", StringComparison.Ordinal);
            return questionMarkIndex != -1 ? categoryHref.Substring(0,questionMarkIndex) : categoryHref;
        }
    }
}