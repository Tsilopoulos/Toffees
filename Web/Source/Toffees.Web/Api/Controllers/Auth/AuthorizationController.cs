using System.Collections.Generic;
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
    [Route("api/Authorization")]
    [ValidateModel]
    public class AuthorizationController : Controller
    {
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> PostTaskAsync()
        {
            var formUrlEncodedContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", "angular"),
                new KeyValuePair<string, string>("client_secret", "93A905DE-7760-4E00-AFC9-B421820F6B70"),
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("scope", "biometric_api.full_access")
            });
            using (var client = new HttpClient())
            {
                var result = await client.PostAsync("http://localhost:5000/connect/token", formUrlEncodedContent).ConfigureAwait(false);
                if (!result.IsSuccessStatusCode) return Unauthorized();
                var identityServerTokenEndpointResponse = JsonConvert.DeserializeObject<IdentityServerJwtResponse>(
                    await result.Content.ReadAsStringAsync().ConfigureAwait(false));
                return Content(identityServerTokenEndpointResponse.AccessToken);
            }
        }
    }
}