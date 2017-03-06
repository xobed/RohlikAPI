using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace RohlikAPI.Helpers
{
    public class PriceParser
    {
        public double ParsePrice(string priceString)
        {
            var cleanPriceString = Regex.Match(priceString, @"(\d*?,\d*)|(\d+)").Value;
            try
            {
                var price = double.Parse(cleanPriceString, new CultureInfo("cs-CZ"));
                return price;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to parse product price from string '{priceString}'. Exception: {ex}");
            }
        }
    }
}