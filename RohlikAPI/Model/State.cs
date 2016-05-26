using Newtonsoft.Json;

namespace RohlikAPI.Model
{
    public class State
    {
        public object orderBy { get; set; }
        public object categoryId { get; set; }
        public object desc { get; set; }
        public object sales { get; set; }
        public object favourites { get; set; }
        public object backlink { get; set; }
        public object locale { get; set; }

        [JsonProperty("brands-filter")]
        public object BrandsFilter { get; set; }

        [JsonProperty("filters-filter")]
        public object FilterFilter { get; set; }

        [JsonProperty("paginator-page")]
        public int? PaginatorPage { get; set; }
    }
}