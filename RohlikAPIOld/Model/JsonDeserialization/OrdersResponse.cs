using Newtonsoft.Json;

namespace RohlikAPI.Model.JsonDeserialization
{
    public class OrdersResponse
    {
        [JsonProperty("snippets")]
        public OrderSnippets OrderSnippets { get; set; }
    }
}