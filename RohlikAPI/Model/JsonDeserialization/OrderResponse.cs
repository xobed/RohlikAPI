using Newtonsoft.Json;

namespace RohlikAPI.Model.JsonDeserialization
{
    public class OrdersResponse
    {
        [JsonProperty("status")]
        public long Status { get; set; }

        [JsonProperty("messages")]
        public object[] Messages { get; set; }

        [JsonProperty("data")]
        public OrderData Data { get; set; }
    }

    public class OrderData
    {
        [JsonProperty("orders")]
        public Order[] Orders { get; set; }
    }

    public class Order
    {
        [JsonProperty("date")]
        public long Date { get; set; }

        [JsonProperty("orderDate")]
        public string OrderDate { get; set; }

        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("orderPrice")]
        public double OrderPrice { get; set; }

        [JsonProperty("benefits")]
        public object[] Benefits { get; set; }
    }
}