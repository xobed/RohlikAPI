using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RohlikAPI.Helpers;

namespace RohlikAPI.Model.JsonDeserialization
{
    public class CategoriesResponse
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("messages")]
        public IList<object> Messages { get; set; }

        [JsonProperty("data")]
        public BlockData Data { get; set; }
    }

    public class Block
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }

        [JsonProperty("isLastOne")]
        public bool? IsLastOne { get; set; }
    }

    public class BlockData
    {
        [JsonProperty("blocks")]
        public IList<Block> Blocks { get; set; }
    }

    public class Badge
    {
        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("dataTip")]
        public string DataTip { get; set; }
    }

    public class Composition
    {
        [JsonProperty("additiveScoreMax")]
        public long AdditiveScoreMax { get; set; }

        [JsonProperty("withoutAdditives")]
        public bool WithoutAdditives { get; set; }

        [JsonProperty("nutritionalValues", NullValueHandling = NullValueHandling.Ignore)]
        public NutritionalValues NutritionalValues { get; set; }

        [JsonProperty("withoutE250E251")]
        public bool WithoutE250E251 { get; set; }

        [JsonProperty("harmfulnessScore", NullValueHandling = NullValueHandling.Ignore)]
        public long? HarmfulnessScore { get; set; }
    }

    public class NutritionalValues
    {
        [JsonProperty("dose")]
        public string Dose { get; set; }

        [JsonProperty("energyValueKJ")]
        public long EnergyValueKj { get; set; }

        [JsonProperty("energyValueKcal")]
        public double? EnergyValueKcal { get; set; }

        [JsonProperty("fats")]
        public double? Fats { get; set; }

        [JsonProperty("saturatedFattyAcids")]
        public double? SaturatedFattyAcids { get; set; }

        [JsonProperty("carbohydrates")]
        public double? Carbohydrates { get; set; }

        [JsonProperty("sugars")]
        public double? Sugars { get; set; }

        [JsonProperty("proteins")]
        public double? Proteins { get; set; }

        [JsonProperty("salt")]
        public double? Salt { get; set; }

        [JsonProperty("fiber")]
        public double? Fiber { get; set; }
    }

    public class Country
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("nameId")]
        public string NameId { get; set; }
    }

    public class OriginalPrice
    {
        [JsonProperty("full")]
        public long Full { get; set; }

        [JsonProperty("whole")]
        public long Whole { get; set; }

        [JsonProperty("fraction")]
        public long Fraction { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }

    public class Price
    {
        [JsonProperty("full")]
        public double Full { get; set; }

        [JsonProperty("whole")]
        public long Whole { get; set; }

        [JsonProperty("fraction")]
        public long Fraction { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }

    public class ProductStory
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("imagePath")]
        public Uri ImagePath { get; set; }

        [JsonProperty("cardColor")]
        public string CardColor { get; set; }

        [JsonProperty("textColor")]
        public string TextColor { get; set; }

        [JsonProperty("bulletText")]
        public string BulletText { get; set; }
    }

    public class Sale
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("promoted")]
        public bool Promoted { get; set; }

        [JsonProperty("remaining")]
        public long Remaining { get; set; }

        [JsonProperty("price")]
        public Price Price { get; set; }

        [JsonProperty("priceForUnit")]
        public Price PriceForUnit { get; set; }

        [JsonProperty("originalPrice")]
        public Price OriginalPrice { get; set; }

        [JsonProperty("discountPercentage")]
        public long DiscountPercentage { get; set; }

        [JsonProperty("discountPrice")]
        public Price DiscountPrice { get; set; }

        [JsonProperty("multipackNeedAmount")]
        public object MultipackNeedAmount { get; set; }

        [JsonProperty("multipackAsPriceDiff")]
        public bool MultipackAsPriceDiff { get; set; }

        [JsonProperty("asPriceDiff")]
        public bool AsPriceDiff { get; set; }

        [JsonProperty("startedAt")]
        public long StartedAt { get; set; }

        [JsonProperty("endsAt")]
        public long EndsAt { get; set; }

        [JsonProperty("originalSemiCaliberPricePerKg")]
        public string OriginalSemiCaliberPricePerKg { get; set; }

        [JsonProperty("codeActivationId")]
        public object CodeActivationId { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("soonExpirationType")]
        public string SoonExpirationType { get; set; }

        [JsonProperty("soonExpirationTypeId")]
        public long? SoonExpirationTypeId { get; set; }

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

    class ProductsSection
    {
        [JsonProperty("products")]
        public List<RohlikProduct> Products { get; set; }

    }
}