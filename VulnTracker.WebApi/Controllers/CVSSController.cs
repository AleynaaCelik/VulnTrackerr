using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VulnTracker.Business.Helpers;
using VulnTracker.Domain.Models;

namespace VulnTracker.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CVSSController : ControllerBase
    {
        // CVSS Hesaplama Endpoint
        [HttpPost("calculate")]
        public IActionResult CalculateCVSS([FromBody] CVSSRequest request)
        {
            if (request.BaseScore <= 0 || request.TemporalScore <= 0 || request.EnvironmentalScore <= 0)
            {
                return BadRequest("All scores must be greater than 0.");
            }

            var result = CVSSHelper.CalculateCVSS(request);
            return Ok(result);
        }
    }
}
