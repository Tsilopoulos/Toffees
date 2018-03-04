using Newtonsoft.Json;

namespace Toffees.Web.Api.Models.Auth
{
    public class ClientAuthorizationCredentials
    {
        [JsonProperty("clientId")]
        public string ClientId { get; set; }

        [JsonProperty("clientSecret")]
        public string ClientSecret { get; set; }

        [JsonProperty("grantType")]
        public string GrantType { get; set; }

        [JsonProperty("scope")]
        public string[] Scope { get; set; }
    }
}
