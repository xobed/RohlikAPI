using System.Collections.Generic;

namespace RohlikAPI.Model.JsonDeserialization
{
    public class RohlikProduct
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public long MainCategoryId { get; set; }
        public string ImgPath { get; set; }
        public string BaseLink { get; set; }
        public string Link { get; set; }
        public long? ExpectedAvailability { get; set; }
        public bool ExpectedAvailabilityHasTime { get; set; }
        public object UnavailabilityReason { get; set; }
        public bool PreorderEnabled { get; set; }
        public string Unit { get; set; }
        public string TextualAmount { get; set; }
        public bool Semicaliber { get; set; }
        public string Currency { get; set; }
        public double Price { get; set; }
        public double PricePerUnit { get; set; }
        public double? OriginalPrice { get; set; }
        public bool GoodPrice { get; set; }
        public long GoodPriceSalePercentage { get; set; }
        public List<Sale> Sales { get; set; }
        public object MyFivePriceDao { get; set; }
        public long MaxBasketAmount { get; set; }
        public List<string> Tags { get; set; }
        public List<Badge> Badge { get; set; }
        public long FavouriteCount { get; set; }
        public Stars Stars { get; set; }
        public Country Country { get; set; }
        public long OrderCount { get; set; }
        public bool InStock { get; set; }
        public bool Favourite { get; set; }
        public bool MyFive { get; set; }
    }

    
}