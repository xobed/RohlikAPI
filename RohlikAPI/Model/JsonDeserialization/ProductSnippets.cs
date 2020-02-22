using Newtonsoft.Json;

namespace RohlikAPI.Model.JsonDeserialization
{
    public class ProductSnippets
    {
        [JsonProperty("snippet--products")]
        public string SnippetProducts { get; set; }

        [JsonProperty("snippet-paginator-loadMore")]
        public string SnippetPaginatorLoadMore { get; set; }
    }
}