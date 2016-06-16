using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Newtonsoft.Json;
using RohlikAPI.Model;

namespace RohlikAPI
{
    public class Products
    {
        private const string BaseUrl = "https://www.rohlik.cz/";
        private readonly PersistentSessionHttpClient httpClient;

        internal Products(PersistentSessionHttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        internal IEnumerable<Product> Get(string category)
        {
            var allProductsString = GetAllProductsString(category);
            var allProductsDocument = new HtmlDocument();
            allProductsDocument.LoadHtml(allProductsString);

            var parsedProducts = ParseProducts(allProductsDocument);

            return parsedProducts;
        }

        private IEnumerable<Product> ParseProducts(HtmlDocument document)
        {
            var productNodes = document.DocumentNode.SelectNodes(@"//*[@class='productGridWrapper']/div[contains(@class,'baseProduct')]//div[@class='productContainerWrap']");
            var parsedProducts = productNodes.Select(GetProductFromNode).ToList();
            parsedProducts.RemoveAll(p => p == null);
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

            var aNode = productNode.SelectSingleNode("*/h3/a");
            
            product.Name = aNode.InnerText;

            const string rohlikUrl = "https://rohlik.cz";
            product.ProductUrl = $"{rohlikUrl}{aNode.Attributes["href"].Value}";

            var priceNode = productNode.SelectSingleNode("*/div/h6/strong");
            product.Price = ParsePrice(priceNode);

            var discountPriceNode = productNode.SelectSingleNode("*/div/h6/del");

            if (discountPriceNode != null)
            {
                product.IsDiscounted = true;
                product.OriginalPrice = ParsePrice(discountPriceNode);
                var dateTimeNode = productNode.SelectSingleNode(".//*[@class='center-content-bot']");
                product.DiscountedUntil = GetDateUntilDiscounted(dateTimeNode);
            }
            else
            {
                product.IsDiscounted = false;
            }

            return product;
        }

        private bool IsSoldOut(HtmlNode productNode)
        {
            var soldOutMessageNode = productNode.SelectSingleNode(".//*[@class='naMessage']");
            return soldOutMessageNode != null;
        }

        private double ParsePrice(HtmlNode priceNode)
        {
            if (priceNode == null) throw new ArgumentNullException(nameof(priceNode));

            var priceString = priceNode.InnerText;

            var cleanPriceString = Regex.Match(priceString, @"\d*?,\d*").Value;
            try
            {
                var price = double.Parse(cleanPriceString);
                return price;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to parse product price from string '{priceString}'. Exception: {ex}");
            }
        }

        private DateTime GetDateUntilDiscounted(HtmlNode dateTimeNode)
        {
            var dateTimeString = dateTimeNode.InnerText;
            dateTimeString = dateTimeString.Replace("Do", "");
            dateTimeString = Regex.Replace(dateTimeString, @"\s", "");
            return DateTime.Parse(dateTimeString);
        }

        private string GetAllProductsString(string category)
        {
            var allProductsString = string.Empty;

            var page = 0;
            var response = GetProductsForPage(category, page);
            allProductsString += CleanProductResults(response.Snippets.SnippetProducts);

            while (response.Snippets.SnippetPaginatorLoadMore != string.Empty)
            {
                page++;
                response = GetProductsForPage(category, page);
                allProductsString += CleanProductResults(response.Snippets.SnippetProducts);
            }

            return allProductsString;
        }

        private ProductResponse GetProductsForPage(string category, int page)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}{category}");
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