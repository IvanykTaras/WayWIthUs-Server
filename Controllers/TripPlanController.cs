using GoogleApi.Entities.Translate.Common.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using WayWIthUs_Server.Data;
using WayWIthUs_Server.Entities;
using WayWIthUs_Server.Service;

namespace WayWIthUs_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripPlanController : ControllerBase
    {
        private readonly IMongoCollection<TripPlan> _tripPlan;
        private readonly IGooglePlacesService _googlePlacesService;
        public TripPlanController(MongoDbService mongoDbService, IConfiguration configuration, IGooglePlacesService googlePlacesService)
        {
            var tripPlanCollectionName = configuration.GetConnectionString("TripPlanCollection");
            _tripPlan = mongoDbService.Database.GetCollection<TripPlan>(tripPlanCollectionName);
            _googlePlacesService = googlePlacesService;
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

            foreach (var hotel in tp.Hotels)
            {
                hotel.image_url = (await _googlePlacesService.getPhotoUrls(hotel.name, 400, 400)).FirstOrDefault();
            }

            foreach (var day in tp.Itinerary)
            {
                foreach (var place in day.Places)
                {
                    place.image_url = (await _googlePlacesService.getPhotoUrls(place.location, 400, 400)).FirstOrDefault();
                }
            }


            await _tripPlan.InsertOneAsync(tp);
            return CreatedAtAction(nameof(GetById), new { id = tp.Id }, tp);
        }
    }
}
