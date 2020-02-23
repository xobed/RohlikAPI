using Newtonsoft.Json;

namespace RohlikAPI.Model.JsonDeserialization
{
    public class SetAddressRequest
    {
        [JsonProperty("streetWithNumber")]
        public string StreetWithNumber { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("isGeocodeResult")]
        public bool IsGeocodeResult { get; set; }
    }
}