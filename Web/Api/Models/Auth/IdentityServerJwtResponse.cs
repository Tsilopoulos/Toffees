using Newtonsoft.Json;

namespace Toffees.Web.Api.Models.Auth
{
    public class IdentityServerJwtResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }
    }
}
