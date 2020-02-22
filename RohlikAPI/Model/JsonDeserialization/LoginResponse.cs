using Newtonsoft.Json;

namespace RohlikAPI.Model.JsonDeserialization
{
    public class LoginResponse
    {
        [JsonProperty("status")] public long Status { get; set; }

        [JsonProperty("messages")] public Message[] Messages { get; set; }

        [JsonProperty("data")] public LoginData Data { get; set; }
    }

    public class LoginData
    {
        [JsonProperty("storeId")] public object StoreId { get; set; }

        [JsonProperty("status")] public long Status { get; set; }

        [JsonProperty("fbLoginUrl")] public object FbLoginUrl { get; set; }

        [JsonProperty("user")] public object User { get; set; }

        [JsonProperty("address")] public object Address { get; set; }

        [JsonProperty("almostDone")] public object AlmostDone { get; set; }

        [JsonProperty("features")] public string[] Features { get; set; }

        [JsonProperty("isAuthenticated")] public bool IsAuthenticated { get; set; }
    }

    public class Message
    {
        [JsonProperty("content")] public string Content { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("context")] public string Context { get; set; }

        [JsonProperty("messageCode")] public string MessageCode { get; set; }
    }
}