using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace RohlikAPISharp
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
            // Replace any newlines and spaces with space
            resultString = resultString.Replace("\n", " ").Replace("&nbsp;", " ");
            resultString = resultString.Replace("\t", "");
            // Trim any other HTML expressions
            resultString = Regex.Replace(resultString, @"(<.*?>)|(&.*?;)", "");
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
