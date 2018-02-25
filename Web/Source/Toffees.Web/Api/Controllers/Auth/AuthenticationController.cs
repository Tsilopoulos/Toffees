using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Toffees.Web.Api.Models;
using Toffees.Web.Api.Validators;

namespace Toffees.Web.Api.Controllers.Auth
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ValidateModel]
    public class AuthenticationController : Controller
    {
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> PostTaskAsync([FromBody]UserAuthCredentials credentials)
        {
            var formUrlEncodedContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", "apiGateway"),
                new KeyValuePair<string, string>("client_secret", "C309386B-ADFA-4A24-B693-2B211806204C"),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", credentials.Username),
                new KeyValuePair<string, string>("password", credentials.Password),
                new KeyValuePair<string, string>("scope", "openid")
            });
            using (var client = new HttpClient())
            {
                var result = await client.PostAsync("http://localhost:5000/connect/token", formUrlEncodedContent).ConfigureAwait(false);
                if (!result.IsSuccessStatusCode) return Unauthorized();
                var identityServerTokenEndpointResponse = JsonConvert.DeserializeObject<IdentityServerJwtResponse>(
                    await result.Content.ReadAsStringAsync().ConfigureAwait(false));
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(identityServerTokenEndpointResponse.AccessToken);
                return Content(token.Subject);
            }
        }
    }
}