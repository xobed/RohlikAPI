using System;

namespace RohlikAPI.Model
{
    public class Product
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public bool IsDiscounted { get; set; }
        public double? OriginalPrice { get; set; }
        public DateTime? DiscountedUntil { get; set; }
        public string ProductUrl { get; set; }
    }
}