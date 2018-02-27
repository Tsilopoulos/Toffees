using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Toffees.Web.Api.Models.Glucose;
using Toffees.Web.Api.Validators;

namespace Toffees.Web.Api.Controllers.Glucose
{
    [AllowAnonymous]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ValidateModel]
    public class GlucoseController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromHeader]string accessToken, [FromRoute]string userId, [FromQuery]string dateTimeRange = null)
        {
            var payload = new StringContent(userId, Encoding.UTF8);
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Bearer ", accessToken);
                var result = await client.GetAsync($"http://localhost:5001/api/biometric/glucose/{payload}").ConfigureAwait(false);
                if (!result.IsSuccessStatusCode) return Unauthorized();
                return Content(await result.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromRoute]string glucoseId)
        {
            using (var client = new HttpClient())
            {
                //set Bearer token based auth
                var result = await client.GetAsync($"http://localhost:5001/api/biometric/glucose/{glucoseId}").ConfigureAwait(false);
                if (!result.IsSuccessStatusCode) return Unauthorized();
                return Content(await result.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromRoute]string userId, [FromBody]GlucoseDto model)
        {
            var payload = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8);
            using (var client = new HttpClient())
            {
                //set Bearer token based auth
                var result = await client.PostAsync($"http://localhost:5001/api/biometric/glucose/{userId}", payload).ConfigureAwait(false);
                if (!result.IsSuccessStatusCode) return Unauthorized();
                return Content(await result.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromRoute]string glucoseId)
        {
            using (var client = new HttpClient())
            {
                //set Bearer token based auth
                var result = await client.DeleteAsync($"http://localhost:5001/api/biometric/glucose/{glucoseId}").ConfigureAwait(false);
                if (!result.IsSuccessStatusCode) return Unauthorized();
                return Ok();
            }
        }
    }
}