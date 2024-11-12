using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WayWIthUs_Server.Service;

namespace WayWIthUs_Server.Controllers
{
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
        public async Task<IActionResult> GenerateTripJson()
        {
            try
            {
                var trip = new GeminiTrip
                {
                    UserEmail = "user@example.com",
                    Location = "Paris",
                    DaysNumber = "1",
                    BudgetType = BudgetType.Cheap,
                    GroupType = GroupType.Family
                };
                var response = await _geminiService.GenerateTripJson(trip);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        
    }
}
