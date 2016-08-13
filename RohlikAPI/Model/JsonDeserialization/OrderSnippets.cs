using Newtonsoft.Json;

namespace RohlikAPI.Model.JsonDeserialization
{
    public class OrderSnippets
    {
        [JsonProperty("snippet--editing")]
        public string SnippetEditing { get; set; }

        [JsonProperty("snippet--changePassword")]
        public string SnippetChangePassword { get; set; }

        [JsonProperty("snippet--changeBillingInfo")]
        public string SnippetChangeBillingInfo { get; set; }

        [JsonProperty("snippet--flashMessages")]
        public string SnippetFlashMessages { get; set; }

        [JsonProperty("snippet--productPopup")]
        public string SnippetProductPopup { get; set; }

        [JsonProperty("snippet-ordersHistoryGrocery-")]
        public string SnippetOrdersHistoryGrocery { get; set; }

        [JsonProperty("snippet-ordersHistoryGrocery-browseOrders")]
        public string SnippetOrdersHistoryGroceryBrowseOrders { get; set; }
    }
}