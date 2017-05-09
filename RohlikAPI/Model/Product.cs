using System;
using HtmlAgilityPack;

namespace RohlikAPI.Model
{
    public class Product
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public double PricePerUnit { get; set; }
        public string Unit { get; set; }
        public bool IsDiscounted { get; set; }
        public double? OriginalPrice { get; set; }
        public DateTime? DiscountedUntil { get; set; }
        public string ProductUrl { get; set; }
        public string ImageUrl { get; set; }
    }
}