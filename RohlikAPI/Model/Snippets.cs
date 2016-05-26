using Newtonsoft.Json;

namespace RohlikAPI.Model
{
    public class Snippets
    {
        [JsonProperty("snippet--products")]
        public string snippetProducts { get; set; }

        [JsonProperty("snippet--flashMessages")]
        public string snippetFlashMessages { get; set; }

        [JsonProperty("snippet--productPopup")]
        public string snippetProductPopup { get; set; }

        [JsonProperty("snippet-paginator-loadMore")]
        public string snippetPaginatorLoadMore { get; set; }
    }
}