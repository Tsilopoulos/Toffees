using Newtonsoft.Json;

namespace Toffees.Web.Api.Models.Auth
{
    public class ClientAuthorizationCredentials
    {
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }

        [JsonProperty("grant_type")]
        public string GrantType { get; set; }

        [JsonProperty("scope")]
        public string[] Scope { get; set; }
    }
}
