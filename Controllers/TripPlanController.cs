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
            // write for me loop which executes ten times
            var filter = Builders<TripPlan>.Filter.Eq(e => e.Id, id);
            return await _tripPlan.Find(filter).FirstOrDefaultAsync();
        }

        [HttpGet("getByEmail")]
        public async Task<List<TripPlan>> GetByEmail([FromQuery]string email)
        {
            var filter = Builders<TripPlan>.Filter.Eq(e => "some@email.com", email);
            return await _tripPlan.Find(filter).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post(TripPlan tp)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var city in tp.CityPlans)
            {
                city.Image_url.OriginUrl = (await _googlePlacesService.getPhotoUrls(city.OriginLocation, 400, 400)).FirstOrDefault();
                city.Image_url.DestinationUrl = (await _googlePlacesService.getPhotoUrls(city.DescriptionLocation, 400, 400)).FirstOrDefault();

            }

            foreach (var city in tp.CityPlans)
            {
                foreach (var hotel in city.Hotels)
                {
                    hotel.image_url = (await _googlePlacesService.getPhotoUrls(hotel.name, 400, 400)).FirstOrDefault();
                    hotel.googleMapUrl = await _googlePlacesService.getPlaceLink(hotel.name);
                }
            }


            
            foreach (var city in tp.CityPlans)
            {
                foreach (var day in city.Itinerary)
                {
                    foreach (var place in day.Places)
                    {
                        place.googleMapUrl = await _googlePlacesService.getPlaceLink(place.location);
                        place.image_url = (await _googlePlacesService.getPhotoUrls(place.location, 400, 400)).FirstOrDefault();
                    }
                }
            }    
            


            await _tripPlan.InsertOneAsync(tp);
            return CreatedAtAction(nameof(GetById), new { id = tp.Id }, tp);
        }
    }
}
