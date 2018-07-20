using System;
using RohlikAPI.Helpers;

namespace RohlikAPI.Model.JsonDeserialization
{
    public class Sale
    {
        public long Id { get; set; }
        public SaleType Type { get; set; }
        public bool Promoted { get; set; }
        public long AmountForSale { get; set; }
        public long Remaining { get; set; }
        public long Price { get; set; }
        public long PriceForUnit { get; set; }
        public double OriginalPrice { get; set; }
        public long DiscountPercentage { get; set; }
        public double DiscountPrice { get; set; }
        public object MultipackNeedAmount { get; set; }
        public bool MultipackAsPriceDiff { get; set; }
        public long StartedAt { get; set; }
        public long EndsAt { get; set; }
        public object OriginalSemiCaliberPricePerKg { get; set; }

        // Manually added
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

    public enum SaleType
    {
        Sale,
        Expiration,
        Multipack
    }
}