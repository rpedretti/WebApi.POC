using Newtonsoft.Json;

namespace WebApi.Shared.Models
{
    public sealed class TokenModel
    {
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }

        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty(PropertyName = "refresh_url")]
        public string RefreshUrl { get; set; }
    }
}
