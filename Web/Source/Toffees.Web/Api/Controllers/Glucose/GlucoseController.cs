using System;
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
        public async Task<IActionResult> Get(string userId, [FromQuery]string dateTimeRange = null)
        {
            var accessToken = Request.Headers["Authorization"].ToString().Substring(7, Request.Headers["Authorization"].ToString().Length - 7);
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

        [HttpPost("{userId}")]
        public async Task<IActionResult> Post(string userId, [FromBody]GlucosePostRequest model)
        {
            var accessToken = Request.Headers["Authorization"].ToString().Substring(7, Request.Headers["Authorization"].ToString().Length - 7);
            var newGlucoseDto = new GlucoseDto
            {
                Data = model.Data,
                PinchDateTime = DateTime.Now,
                Tag = model.Tag
            };
            var payload = new StringContent(JsonConvert.SerializeObject(newGlucoseDto), Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                var result = await client.PostAsync($"http://localhost:5001/api/biometric/glucose/{userId}", payload).ConfigureAwait(false);
                if (!result.IsSuccessStatusCode) return Unauthorized();
                return Created("POST:/api/biometric/glucose", 
                    JsonConvert.DeserializeObject<GlucoseDto>(await result.Content.ReadAsStringAsync().ConfigureAwait(false)));
            }
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> Put(string userId, [FromBody]GlucoseDto model)
        {
            var accessToken = Request.Headers["Authorization"].ToString().Substring(7, Request.Headers["Authorization"].ToString().Length - 7);
            var newGlucoseDto = new GlucoseDto
            {
                Id = model.Id,
                Data = model.Data,
                PinchDateTime = DateTime.Now,
                Tag = model.Tag
            };
            var payload = new StringContent(JsonConvert.SerializeObject(newGlucoseDto), Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                var result = await client.PutAsync($"http://localhost:5001/api/biometric/glucose/{userId}", payload).ConfigureAwait(false);
                if (!result.IsSuccessStatusCode) return Unauthorized();
                return Ok(JsonConvert.DeserializeObject<GlucoseDto>(await result.Content.ReadAsStringAsync().ConfigureAwait(false)));
            }
        }

        [HttpDelete("{glucoseId}")]
        public async Task<IActionResult> Delete(int glucoseId)
        {
            var accessToken = Request.Headers["Authorization"].ToString().Substring(7, Request.Headers["Authorization"].ToString().Length - 7);
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                var result = await client.DeleteAsync($"http://localhost:5001/api/biometric/glucose/{glucoseId}").ConfigureAwait(false);
                if (!result.IsSuccessStatusCode) return Unauthorized();
                return Ok();
            }
        }
    }
}