using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
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
            var productNodes = document.DocumentNode.SelectNodes(@"//*[@class='product__grid_wrapper']/div[contains(@class,'base_product')]/article/div/div[contains(@class,'product__order')]");
            var parsedProducts = productNodes.AsParallel().Select(GetProductFromNode).Where(p => p != null);
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

            var rawName = aNode.InnerText.Trim();
            product.Name = WebUtility.HtmlDecode(rawName);

            var imageNode = productNode.SelectSingleNode(".//header/a/img");
            var imageUrlLarge = imageNode.Attributes["data-replace"].Value;
            var imageUrlSmall = imageUrlLarge.Replace("260.jpg", "160.jpg");

            product.ImageUrl = imageUrlSmall;

            const string rohlikUrl = "https://rohlik.cz";
            product.ProductUrl = $"{rohlikUrl}{aNode.Attributes["href"].Value}";

            var priceNode = productNode.SelectSingleNode(".//div/strong[contains(@class,'font-15')]");
            product.Price = priceParser.ParsePrice(priceNode.InnerText);

            var pricePerUnitNode = productNode.SelectSingleNode(".//span[@class='grey font-11']/text()");
            var pricePerUnitString = pricePerUnitNode.InnerText.Trim().Trim('(',')');

            product.PricePerUnit = priceParser.ParsePrice(pricePerUnitString);
            product.Unit = pricePerUnitString.Split(new[] { "&nbsp;" }, StringSplitOptions.None).Last();

            var discountPriceNode = productNode.SelectSingleNode(".//span[@class='grey font-11']/del");

            if (discountPriceNode != null)
            {
                product.IsDiscounted = true;
                product.OriginalPrice = priceParser.ParsePrice(discountPriceNode.InnerText);
                var dateTimeNode = productNode.SelectSingleNode(".//div[@class='action-red action font-13']");
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
            const string notAvailableClass = "--not-available";
            return productNode.InnerHtml.Contains(notAvailableClass);
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
                if (dateTimeString.ToLower().Contains("výhodnácena"))
                {
                    return null;
                }
                throw new FormatException($"Failed to parse datetime string: '{dateTimeString}'. Ex: {ex}");
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

        private string GetProductsForPageString(string category, int page, string baseUrl)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}{category}");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");

            var parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("do", "paginator-loadMore"),
                new KeyValuePair<string, string>("paginator-page", page.ToString())
            };
            return httpClient.Get(request, parameters.ToArray());
        }

        private ProductResponse GetProductsForPage(string category, int page, string baseUrl)
        {
            int retriesLeft = 5;
            const string errorResponse = "{\"error\":true}";
            string stringResponse = errorResponse;

            while (retriesLeft > 0 && (stringResponse == errorResponse || stringResponse == String.Empty))
            {
                stringResponse = GetProductsForPageString(category, page, baseUrl);
                var result = JsonConvert.DeserializeObject<ProductResponse>(stringResponse);
                if (result?.ProductSnippets != null)
                {
                    return result;
                }

                retriesLeft--;
            }
            throw new Exception($"Failed to get products for page {page} of category {category}");
        }

        private string CleanProductResults(string productString)
        {
            if (string.IsNullOrEmpty(productString)) throw new ArgumentException(productString);

            var cleanedString = productString.Replace("\n", "").Replace("\t", "").Replace(@"\""", @"""");
            return cleanedString;
        }
    }
}