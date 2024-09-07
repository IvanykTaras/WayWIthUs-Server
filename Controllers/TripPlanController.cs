using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using WayWIthUs_Server.Data;
using WayWIthUs_Server.Entities;

namespace WayWIthUs_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripPlanController : ControllerBase
    {
        private readonly IMongoCollection<TripPlan> _tripPlan;
        public TripPlanController(MongoDbService mongoDbService, IConfiguration configuration)
        {
            var tripPlanCollectionName = configuration.GetConnectionString("TripPlanCollection");
            _tripPlan = mongoDbService.Database.GetCollection<TripPlan>(tripPlanCollectionName);
        }

        [HttpGet]
        public async Task<List<TripPlan>> Get()
        {
            return await _tripPlan.Find(FilterDefinition<TripPlan>.Empty).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<TripPlan> GetById(string id)
        {
            var filter = Builders<TripPlan>.Filter.Eq(e => e.Id, id);
            return await _tripPlan.Find(filter).FirstOrDefaultAsync();
        }

        [HttpGet("getByEmail")]
        public async Task<List<TripPlan>> GetByEmail([FromQuery]string email)
        {
            var filter = Builders<TripPlan>.Filter.Eq(e => e.UserEmail, email);
            return await _tripPlan.Find(filter).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post(TripPlan tp)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _tripPlan.InsertOneAsync(tp);
            return CreatedAtAction(nameof(GetById), new { id = tp.Id }, tp);
        }
    }
}
