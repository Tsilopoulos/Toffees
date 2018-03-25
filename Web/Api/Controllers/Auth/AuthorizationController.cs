using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Toffees.Web.Api.Models.Auth;
using Toffees.Web.Api.Validators;

namespace Toffees.Web.Api.Controllers.Auth
{
    [AllowAnonymous]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ValidateModel]
    public class AuthorizationController : Controller
    {
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> PostTaskAsync([FromBody]ClientAuthorizationCredentials model)
        {
            var formUrlEncodedContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", model.ClientId),
                new KeyValuePair<string, string>("client_secret", model.ClientSecret),
                new KeyValuePair<string, string>("grant_type", model.GrantType),
                new KeyValuePair<string, string>("scope", model.Scope[0])
            });
            using (var client = new HttpClient())
            {
                var result = await client.PostAsync("http://localhost:5000/connect/token", formUrlEncodedContent).ConfigureAwait(false);
                if (!result.IsSuccessStatusCode) return Unauthorized();
                var identityServerTokenEndpointResponse = JsonConvert.DeserializeObject<IdentityServerJwtResponse>(
                    await result.Content.ReadAsStringAsync().ConfigureAwait(false));
                return new JsonResult(identityServerTokenEndpointResponse);
            }
        }
    }
}