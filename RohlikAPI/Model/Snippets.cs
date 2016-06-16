using Newtonsoft.Json;

namespace RohlikAPI.Model
{
    public class Snippets
    {
        [JsonProperty("snippet--products")]
        public string SnippetProducts { get; set; }

        [JsonProperty("snippet-paginator-loadMore")]
        public string SnippetPaginatorLoadMore { get; set; }
    }
}