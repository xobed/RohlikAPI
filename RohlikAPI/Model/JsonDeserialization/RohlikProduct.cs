using System;
using Newtonsoft.Json;
using RohlikAPI.Helpers;

namespace RohlikAPI.Model.JsonDeserialization
{
    public class Temperatures
    {
        [JsonProperty("status")] public long Status { get; set; }

        [JsonProperty("messages")] public object[] Messages { get; set; }

        [JsonProperty("data")] public Data Data { get; set; }
    }


    public class RohlikProduct
    {
        [JsonProperty("productId")] public long ProductId { get; set; }

        [JsonProperty("productName")] public string ProductName { get; set; }

        [JsonProperty("mainCategoryId")] public long MainCategoryId { get; set; }

        [JsonProperty("imgPath")] public string ImgPath { get; set; }

        [JsonProperty("baseLink")] public string BaseLink { get; set; }

        [JsonProperty("link")] public string Link { get; set; }

        [JsonProperty("expectedAvailability")] public long? ExpectedAvailability { get; set; }

        [JsonProperty("expectedAvailabilityHasTime")]
        public bool ExpectedAvailabilityHasTime { get; set; }

        [JsonProperty("unavailabilityReason")] public string UnavailabilityReason { get; set; }

        [JsonProperty("preorderEnabled")] public bool PreorderEnabled { get; set; }

        [JsonProperty("unit")] public string Unit { get; set; }

        [JsonProperty("textualAmount")] public string TextualAmount { get; set; }

        [JsonProperty("semicaliber")] public bool Semicaliber { get; set; }

        [JsonProperty("currency")] public string Currency { get; set; }

        [JsonProperty("price")] public PriceDetail Price { get; set; }

        [JsonProperty("pricePerUnit")] public PriceDetail PricePerUnit { get; set; }

        [JsonProperty("recommendedPricePerUnit")]
        public PriceDetail RecommendedPriceDetailPerUnit { get; set; }

        [JsonProperty("originalPrice")] public PriceDetail OriginalPrice { get; set; }

        [JsonProperty("goodPrice")] public bool GoodPrice { get; set; }

        [JsonProperty("goodPriceSalePercentage")]
        public long GoodPriceSalePercentage { get; set; }

        [JsonProperty("sales")] public Sale[] Sales { get; set; }

        [JsonProperty("maxBasketAmount")] public long MaxBasketAmount { get; set; }

        [JsonProperty("maxBasketAmountReason")]
        public string MaxBasketAmountReason { get; set; }

        [JsonProperty("tags")] public string[] Tags { get; set; }

        [JsonProperty("badge")] public Badge[] Badge { get; set; }

        [JsonProperty("favouriteCount")] public long FavouriteCount { get; set; }

        [JsonProperty("stars")] public Stars Stars { get; set; }

        [JsonProperty("country")] public Country Country { get; set; }

        [JsonProperty("countries")] public Country[] Countries { get; set; }

        [JsonProperty("orderCount")] public long OrderCount { get; set; }

        [JsonProperty("imageScaleRatio")] public long ImageScaleRatio { get; set; }

        [JsonProperty("imagesCount")] public long ImagesCount { get; set; }

        [JsonProperty("immediateConsumption")] public bool ImmediateConsumption { get; set; }

        [JsonProperty("watchDog")] public bool WatchDog { get; set; }

        [JsonProperty("composition")] public Composition Composition { get; set; }

        [JsonProperty("companyId")] public long CompanyId { get; set; }

        [JsonProperty("productStory")] public ProductStory ProductStory { get; set; }

        [JsonProperty("favourite")] public bool Favourite { get; set; }

        [JsonProperty("inStock")] public bool InStock { get; set; }

        [JsonProperty("firstActiveSale")] public Sale FirstActiveSale { get; set; }
    }

    public class Badge
    {
        [JsonProperty("slug")] public string Slug { get; set; }

        [JsonProperty("label")] public string Label { get; set; }

        [JsonProperty("dataTip")] public string DataTip { get; set; }
    }

    public class Composition
    {
        [JsonProperty("additiveScoreMax")] public long AdditiveScoreMax { get; set; }

        [JsonProperty("withoutAdditives", NullValueHandling = NullValueHandling.Ignore)]
        public bool? WithoutAdditives { get; set; }

        [JsonProperty("nutritionalValues", NullValueHandling = NullValueHandling.Ignore)]
        public NutritionalValues NutritionalValues { get; set; }

        [JsonProperty("withoutE250E251")] public bool WithoutE250E251 { get; set; }

        [JsonProperty("harmfulnessScore", NullValueHandling = NullValueHandling.Ignore)]
        public long? HarmfulnessScore { get; set; }
    }

    public class NutritionalValues
    {
        [JsonProperty("dose")] public string Dose { get; set; }

        [JsonProperty("energyValueKJ")] public double? EnergyValueKj { get; set; }

        [JsonProperty("energyValueKcal")] public double? EnergyValueKcal { get; set; }

        [JsonProperty("fats")] public double? Fats { get; set; }

        [JsonProperty("saturatedFattyAcids")] public double? SaturatedFattyAcids { get; set; }

        [JsonProperty("carbohydrates")] public double? Carbohydrates { get; set; }

        [JsonProperty("sugars")] public double? Sugars { get; set; }

        [JsonProperty("proteins")] public double? Proteins { get; set; }

        [JsonProperty("salt")] public double? Salt { get; set; }

        [JsonProperty("fiber")] public double? Fiber { get; set; }
    }

    public class Country
    {
        [JsonProperty("id")] public long Id { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("nameId")] public string NameId { get; set; }
    }

    public class Sale
    {
        [JsonProperty("id")] public long Id { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("promoted")] public bool Promoted { get; set; }

        [JsonProperty("remaining")] public long Remaining { get; set; }

        [JsonProperty("price")] public PriceDetail Price { get; set; }

        [JsonProperty("priceForUnit")] public PriceDetail PriceDetailForUnit { get; set; }

        [JsonProperty("originalPrice")] public PriceDetail OriginalPrice { get; set; }

        [JsonProperty("discountPercentage")] public long DiscountPercentage { get; set; }

        [JsonProperty("discountPrice")] public PriceDetail DiscountPriceDetail { get; set; }

        [JsonProperty("multipackNeedAmount")] public object MultipackNeedAmount { get; set; }

        [JsonProperty("multipackAsPriceDiff")] public bool MultipackAsPriceDiff { get; set; }

        [JsonProperty("asPriceDiff")] public bool AsPriceDiff { get; set; }

        [JsonProperty("startedAt")] public long StartedAt { get; set; }

        [JsonProperty("endsAt")] public long EndsAt { get; set; }

        [JsonProperty("originalSemiCaliberPricePerKg")]
        public string OriginalSemiCaliberPricePerKg { get; set; }

        [JsonProperty("codeActivationId")] public object CodeActivationId { get; set; }

        [JsonProperty("active")] public bool Active { get; set; }

        [JsonProperty("soonExpirationType")] public object SoonExpirationType { get; set; }

        [JsonProperty("soonExpirationTypeId")] public object SoonExpirationTypeId { get; set; }

        public DateTime EndsAtDateTime
        {
            get
            {
                // Rohlik uses milisecond precision for unix timestamps
                var secondsFromEpoch = EndsAt / 1000;
                return DateTimeParser.UnixTimeStampToDateTime(secondsFromEpoch);
            }
        }
    }

    public class PriceDetail
    {
        [JsonProperty("full")] public double Full { get; set; }

        [JsonProperty("whole")] public long Whole { get; set; }

        [JsonProperty("fraction")] public long Fraction { get; set; }

        [JsonProperty("currency")] public string Currency { get; set; }
    }

    public class ProductStory
    {
        [JsonProperty("title")] public string Title { get; set; }

        [JsonProperty("text")] public string Text { get; set; }

        [JsonProperty("imagePath")] public Uri ImagePath { get; set; }

        [JsonProperty("cardColor")] public string CardColor { get; set; }

        [JsonProperty("textColor")] public string TextColor { get; set; }

        [JsonProperty("bulletText")] public string BulletText { get; set; }
    }

    public class Stars
    {
        [JsonProperty("value")] public double Value { get; set; }

        [JsonProperty("count")] public long Count { get; set; }
    }
}