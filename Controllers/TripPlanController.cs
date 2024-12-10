using GoogleApi.Entities.Translate.Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using WayWIthUs_Server.Data;
using WayWIthUs_Server.Entities;
using WayWIthUs_Server.Service;

namespace WayWIthUs_Server.Controllers
{
    [Authorize]
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

        [AllowAnonymous]
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
                city.Image_url = (await _googlePlacesService.getPhotoUrls(city.OriginLocation, 400, 400)).FirstOrDefault();
            }

            foreach (var city in tp.CityPlans)
            {
                foreach (var accomodation in city.Accommodations)
                {
                    accomodation.image_url = (await _googlePlacesService.getPhotoUrls(accomodation.location_acc, 400, 400)).FirstOrDefault();
                    accomodation.googleMapUrl = await _googlePlacesService.getPlaceLink(accomodation.location_acc);
                }
            }
            
            foreach (var city in tp.CityPlans)
            {
                    foreach (var place in city.Places)
                    {
                        place.googleMapUrl = await _googlePlacesService.getPlaceLink(place.location);
                        place.image_url = (await _googlePlacesService.getPhotoUrls(place.location, 400, 400)).FirstOrDefault();
                    }
            }    

            tp.Participants.Add(tp.UserId);

            await _tripPlan.InsertOneAsync(tp);
            return CreatedAtAction(nameof(GetById), new { id = tp.Id }, tp);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, TripPlan tp)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var filter = Builders<TripPlan>.Filter.Eq(e => e.Id, id);
            var updateResult = await _tripPlan.ReplaceOneAsync(filter, tp);

            if (updateResult.IsAcknowledged && updateResult.ModifiedCount > 0)
            {
                return NoContent();
            }

            return NotFound();
        }


        [HttpPost("{id}/addParticipant/{participantId}")]
        public async Task<IActionResult> AddParticipant(string id, string participantId)
        {
            var filter = Builders<TripPlan>.Filter.Eq(e => e.Id, id);
            
            var tripPlan = await _tripPlan.Find(filter).FirstOrDefaultAsync();
            if (tripPlan.Participants.Contains(participantId))
            {
                return Ok();
            }

            var update = Builders<TripPlan>.Update.AddToSet("Participants", participantId);
            var updateResult = await _tripPlan.UpdateOneAsync(filter, update);

            if (updateResult.IsAcknowledged && updateResult.ModifiedCount > 0)
            {
                return NoContent();
            }

            return Ok();
        }

        [HttpDelete("{id}/removeParticipant/{participantId}")]
        public async Task<IActionResult> RemoveParticipant(string id, string participantId)
        {
            var filter = Builders<TripPlan>.Filter.Eq(e => e.Id, id);

            var tripPlan = await _tripPlan.Find(filter).FirstOrDefaultAsync();
            if (!tripPlan.Participants.Contains(participantId))
            {
                return Ok();
            }

            var update = Builders<TripPlan>.Update.Pull("Participants", participantId);
            var updateResult = await _tripPlan.UpdateOneAsync(filter, update);

            if (updateResult.IsAcknowledged && updateResult.ModifiedCount > 0)
            {
                return NoContent();
            }

            return Ok();
        }

        
    }
}
