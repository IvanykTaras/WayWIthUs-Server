using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WayWIthUs_Server.Service;

namespace WayWIthUs_Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GeminiController : ControllerBase
    {

        private readonly IGeminiService _geminiService;

        public GeminiController(IGeminiService geminiService)
        {
            _geminiService = geminiService;
        }

        [HttpPost]
        public async Task<IActionResult> GenerateTripJson([FromBody] string message)
        {
            try
            {
                string response = await _geminiService.OnMessage(message);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        
    }
}
