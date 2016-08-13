using System;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;

namespace RohlikAPI
{
    internal class Rohlikovac
    {
        // Rohlikovac result xPath
        private const string RohlikovacResult = "//*[@id='rohlikovac']//h2";
        private const string RohlikovacUrl = "https://www.rohlik.cz/stranka/rohlikovac?do=creditForge-roll";

        private readonly PersistentSessionHttpClient httpSessionClient;

        public Rohlikovac(PersistentSessionHttpClient httpSessionClient)
        {
            this.httpSessionClient = httpSessionClient;
        }

        private string GetRohlikovacResult()
        {
            var rohlikovacResultString = httpSessionClient.Get(RohlikovacUrl);
            var rohlikovacDocument = new HtmlDocument();
            rohlikovacDocument.LoadHtml(rohlikovacResultString);
            var rohlikovacResult = rohlikovacDocument.DocumentNode.SelectSingleNode(RohlikovacResult);

            var resultString = rohlikovacResult.InnerHtml.Trim();
            resultString = HttpUtility.HtmlDecode(resultString);

            // Replace any newlines, tabs and spaces with space
            resultString = resultString.Replace("\n", " ").Replace("\t", " ").Replace("<br>", " ");
            // Trim any other HTML expressions
            resultString = Regex.Replace(resultString, @"(<.*?>)|(&.*?;)", "");
            // Make any doublespaces single space
            resultString = Regex.Replace(resultString, @"[ ]{2,}", " ");
            return resultString;
        }

        public string Run()
        {
            try
            {
                var rohlikovacResult = GetRohlikovacResult();
                return rohlikovacResult;
            }
            catch (Exception ex)
            {
                return $"Failed to run Rohlikovac. Error: {ex.Message}. StackTrace: {ex.StackTrace}. Details: {ex}";
            }
        }
    }
}