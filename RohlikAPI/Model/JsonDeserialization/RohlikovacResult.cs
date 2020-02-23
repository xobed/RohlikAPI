using Newtonsoft.Json;

namespace RohlikAPI.Model.JsonDeserialization
{
    public class RohlikovacResult
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("messages")]
        public object[] Messages { get; set; }

        [JsonProperty("data")]
        public RohlikovacData Data { get; set; }
    }

    public class RohlikovacData
    {
        [JsonProperty("rolled")]
        public bool Rolled { get; set; }

        [JsonProperty("credits")]
        public int Credits { get; set; }
    }
}