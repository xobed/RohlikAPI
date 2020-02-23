using Newtonsoft.Json;

namespace RohlikAPI.Model.JsonDeserialization
{
    public class Message
    {
        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("context")]
        public string Context { get; set; }

        [JsonProperty("messageCode")]
        public string MessageCode { get; set; }
    }
}