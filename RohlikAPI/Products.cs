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
    internal class Products
    {
        private const string BaseUrl = "https://www.rohlik.cz/";
        private const string BaseSearchUrl = "https://www.rohlik.cz/hledat/";
        private readonly PersistentSessionHttpClient httpClient;
        private readonly PriceParser priceParser = new PriceParser();

        private readonly DateTime currentDate = DateTime.Now;

        internal Products(PersistentSessionHttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        internal IEnumerable<Product> Get(string category)
        {
            var allProductsString = GetAllProductsString(category, BaseUrl);
            return GetProductsFromHtmlString(allProductsString);
        }

        internal IEnumerable<Product> Search(string searchString)
        {
            var allProductsString = GetAllProductsString(searchString, BaseSearchUrl);
            return GetProductsFromHtmlString(allProductsString);
        }

        private IEnumerable<Product> GetProductsFromHtmlString(string htmlString)
        {
            var allProductsDocument = new HtmlDocument();
            allProductsDocument.LoadHtml(htmlString);

            var parsedProducts = ParseProducts(allProductsDocument);

            return parsedProducts;
        }

        private IEnumerable<Product> ParseProducts(HtmlDocument document)
        {
            var productNodes = document.DocumentNode.SelectNodes(@"//*[@class='product__grid_wrapper']/div[@class='base_product']//div[@class='product__wrapper']");
            var parsedProducts = productNodes.Select(GetProductFromNode).Where(p => p != null);            
            return parsedProducts;
        }

        private Product GetProductFromNode(HtmlNode productNode)
        {
            if (productNode == null) throw new ArgumentNullException(nameof(productNode));
            if (IsSoldOut(productNode))
            {
                return null;
            }
            var product = new Product();

            var aNode = productNode.SelectSingleNode(".//div/h3/a");

            product.Name = aNode.InnerText.Trim();

            const string rohlikUrl = "https://rohlik.cz";
            product.ProductUrl = $"{rohlikUrl}{aNode.Attributes["href"].Value}";

            var priceNode = productNode.SelectSingleNode(".//div[contains(@class,'tac')]/strong");
            product.Price = priceParser.ParsePrice(priceNode.InnerText);

            var discountPriceNode = productNode.SelectSingleNode(".//div[@class='action tac']/del");

            if (discountPriceNode != null)
            {
                product.IsDiscounted = true;
                product.OriginalPrice = priceParser.ParsePrice(discountPriceNode.InnerText);
                var dateTimeNode = productNode.SelectSingleNode(".//div[@class='circle']/span");
                if (dateTimeNode != null)
                {
                    product.DiscountedUntil = GetDateUntilDiscounted(dateTimeNode);
                }                
            }
            else
            {
                product.IsDiscounted = false;
            }

            return product;
        }

        private bool IsSoldOut(HtmlNode productNode)
        {
            var soldOutMessageNode = productNode.SelectSingleNode(".//div[@class='product__unavailable']");
            return soldOutMessageNode != null;
        }

        private DateTime? GetDateUntilDiscounted(HtmlNode dateTimeNode)
        {
            if (dateTimeNode == null)
            {
                throw new ArgumentNullException(nameof(dateTimeNode));
            }
            var dateTimeString = dateTimeNode.InnerText;
            dateTimeString = dateTimeString.Replace("Akce do ", "");
            dateTimeString = Regex.Replace(dateTimeString, @"\s", "");
            dateTimeString += currentDate.Year;
            
            try
            {
                var dateTime = DateTime.Parse(dateTimeString, new CultureInfo("cs-CZ"));
                // Date is less than current, e.g. 'Akce do 1.1.' displayed on 24.12.
                if (dateTime < currentDate)
                {
                    dateTime = dateTime.AddYears(1);
                }
                return dateTime;
            }
            catch (FormatException ex)
            {
                // Some products are marked as discounted, but do not show discounted until
                if (dateTimeString.Contains("Výhodnácena"))
                {
                    return null;
                }
                throw new FormatException($"Failed to parse datetime string: {dateTimeString}. Ex: {ex}");
            }
        }

        private string GetAllProductsString(string category, string baseUrl)
        {
            var allProductsString = string.Empty;

            var page = 0;
            var response = GetProductsForPage(category, page, baseUrl);
            allProductsString += CleanProductResults(response.ProductSnippets.SnippetProducts);

            while (response.ProductSnippets.SnippetPaginatorLoadMore != string.Empty)
            {
                page++;
                response = GetProductsForPage(category, page, baseUrl);
                allProductsString += CleanProductResults(response.ProductSnippets.SnippetProducts);
            }

            return allProductsString;
        }

        private ProductResponse GetProductsForPage(string category, int page, string baseUrl)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}{category}");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");

            var parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("do", "paginator-loadMore"),
                new KeyValuePair<string, string>("paginator-page", page.ToString())
            };

            var stringResponse = httpClient.Get(request, parameters.ToArray());
            var result = JsonConvert.DeserializeObject<ProductResponse>(stringResponse);

            return result;
        }

        private string CleanProductResults(string productString)
        {
            var cleanedString = productString.Replace("\n", "").Replace("\t", "").Replace(@"\""", @"""");
            return cleanedString;
        }
    }
}