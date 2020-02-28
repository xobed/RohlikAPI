using System.Collections.Generic;
using Newtonsoft.Json;

namespace RohlikAPI.Model.JsonDeserialization
{
    public class SearchResponse
    {
        [JsonProperty("status")]
        public long Status { get; set; }

        [JsonProperty("messages")]
        public List<object> Messages { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }
    }


    public class Category
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("topParentName")]
        public string TopParentName { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("nameLong")]
        public string NameLong { get; set; }

        [JsonProperty("images")]
        public List<string> Images { get; set; }
    }

    public class Company
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("totalHits")]
        public long TotalHits { get; set; }
    }

    public class ProductList
    {
        [JsonProperty("productId")]
        public long ProductId { get; set; }

        [JsonProperty("productName")]
        public string ProductName { get; set; }

        [JsonProperty("mainCategoryId")]
        public long MainCategoryId { get; set; }

        [JsonProperty("imgPath")]
        public string ImgPath { get; set; }

        [JsonProperty("baseLink")]
        public string BaseLink { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("expectedAvailability")]
        public long? ExpectedAvailability { get; set; }

        [JsonProperty("unavailabilityReason")]
        public object UnavailabilityReason { get; set; }

        [JsonProperty("preorderEnabled")]
        public bool PreorderEnabled { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }

        [JsonProperty("textualAmount")]
        public string TextualAmount { get; set; }

        [JsonProperty("semicaliber")]
        public bool Semicaliber { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("price")]
        public OriginalPrice Price { get; set; }

        [JsonProperty("pricePerUnit")]
        public OriginalPrice PricePerUnit { get; set; }

        [JsonProperty("recommendedPricePerUnit")]
        public OriginalPrice RecommendedPricePerUnit { get; set; }

        [JsonProperty("originalPrice")]
        public OriginalPrice OriginalPrice { get; set; }

        [JsonProperty("goodPrice")]
        public bool GoodPrice { get; set; }

        [JsonProperty("goodPriceSalePercentage")]
        public long GoodPriceSalePercentage { get; set; }

        [JsonProperty("sales")]
        public List<object> Sales { get; set; }

        [JsonProperty("maxBasketAmount")]
        public long MaxBasketAmount { get; set; }

        [JsonProperty("maxBasketAmountReason")]
        public string MaxBasketAmountReason { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        [JsonProperty("badge")]
        public List<object> Badge { get; set; }

        [JsonProperty("stars")]
        public object Stars { get; set; }

        [JsonProperty("country")]
        public Country Country { get; set; }

        [JsonProperty("countries")]
        public List<Country> Countries { get; set; }

        [JsonProperty("imageScaleRatio")]
        public long ImageScaleRatio { get; set; }

        [JsonProperty("imagesCount")]
        public long ImagesCount { get; set; }

        [JsonProperty("immediateConsumption")]
        public bool ImmediateConsumption { get; set; }

        [JsonProperty("watchDog")]
        public bool WatchDog { get; set; }

        [JsonProperty("composition")]
        public Composition Composition { get; set; }

        [JsonProperty("companyId")]
        public long CompanyId { get; set; }

        [JsonProperty("productStory")]
        public object ProductStory { get; set; }

        [JsonProperty("vivino")]
        public object Vivino { get; set; }

        [JsonProperty("archived")]
        public bool Archived { get; set; }

        [JsonProperty("inStockByAmount")]
        public bool InStockByAmount { get; set; }

        [JsonProperty("inStock")]
        public bool InStock { get; set; }

        [JsonProperty("favourite")]
        public bool Favourite { get; set; }
    }
}
