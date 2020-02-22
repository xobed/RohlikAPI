using Newtonsoft.Json;

namespace RohlikAPI.Model.JsonDeserialization
{
    public class ProductResponse
    {
        [JsonProperty("snippets")]
        public ProductSnippets ProductSnippets { get; set; }
    }
}