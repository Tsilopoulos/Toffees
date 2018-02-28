using System.Collections.Generic;
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
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAll(string userId, [FromQuery]string dateTimeRange = null)
        {
            var accessToken = Request.Headers["Authorization"].ToString();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                var result = await client.GetAsync($"http://localhost:5001/api/biometric/glucose/{userId}").ConfigureAwait(false);
                if (!result.IsSuccessStatusCode) return Unauthorized();
                var glucoseEnumerable = JsonConvert.DeserializeObject<IEnumerable<GlucoseDto>>(await result.Content.ReadAsStringAsync().ConfigureAwait(false));
                return new JsonResult(glucoseEnumerable);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromRoute]string userId, [FromBody]GlucoseDto model)
        {
            var accessToken = Request.Headers["Authorization"].ToString();
            var payload = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8);
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                var result = await client.PostAsync($"http://localhost:5001/api/biometric/glucose/{userId}", payload).ConfigureAwait(false);
                if (!result.IsSuccessStatusCode) return Unauthorized();
                return Content(await result.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromRoute]int glucoseId)
        {
            var accessToken = Request.Headers["Authorization"].ToString();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                var result = await client.DeleteAsync($"http://localhost:5001/api/biometric/glucose/{glucoseId}").ConfigureAwait(false);
                if (!result.IsSuccessStatusCode) return Unauthorized();
                return Ok();
            }
        }
    }
}