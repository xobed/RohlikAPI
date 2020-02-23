using Newtonsoft.Json;

namespace RohlikAPI.Model.JsonDeserialization
{
    public class SetAddressResponse
    {
        [JsonProperty("status")]
        public long Status { get; set; }

        [JsonProperty("messages")]
        public Message[] Messages { get; set; }

        [JsonProperty("data")]
        public SetAddressData Data { get; set; }
    }

    public class SetAddressData
    {
        [JsonProperty("address")]
        public Address Address { get; set; }

        [JsonProperty("changedItemList")]
        public ChangedItemList ChangedItemList { get; set; }
    }

    public class Address
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("fullAddress")]
        public string FullAddress { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("postalCode")]
        public long PostalCode { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("isDeliveredTo")]
        public bool IsDeliveredTo { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }
    }

    public class ChangedItemList
    {
        [JsonProperty("changedItems")]
        public object[] ChangedItems { get; set; }
    }
}