using System.Globalization;
using System.Text;

namespace RohlikAPIWeb.Models
{
    public class ApiProduct
    {
        public string Name { get; }
        public double Price { get; }
        public double PPU { get; }
        public string Unit { get; }
        public string Sname { get; }
        public string Href { get; }
        public string Img { get; }

        public ApiProduct(string name, double price, double ppu, string unit, string href, string img)
        {
            Name = name;
            Price = price;
            PPU = ppu;
            Unit = unit;
            Href = href;
            Sname = StandardizeString(name);
            Img = img;
        }

        private string StandardizeString(string str)
        {
            var standardizedName = str.ToLower();
            standardizedName = RemoveDiacritics(standardizedName);

            return standardizedName;
        }

        private string RemoveDiacritics(string originalString)
        {
            var normalizedString = originalString.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var character in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(character);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(character);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}