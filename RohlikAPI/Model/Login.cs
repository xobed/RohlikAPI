using Newtonsoft.Json;

namespace RohlikAPI.Model
{
    internal class Login
    {
        [JsonProperty("email")] public string Email { get; set; }

        [JsonProperty("password")] public string Password { get; set; }
    }
}