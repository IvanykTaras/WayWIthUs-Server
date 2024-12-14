using GoogleApi.Entities.Translate.Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using WayWIthUs_Server.Data;
using WayWIthUs_Server.Entities;
using WayWIthUs_Server.Models;
using WayWIthUs_Server.Service;

namespace WayWIthUs_Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TripPlanController : ControllerBase
    {
        private readonly IMongoCollection<TripPlan> _tripPlan;
        private readonly IMongoCollection<User> _user;
        private readonly IGooglePlacesService _googlePlacesService;
        private readonly OpenAiService _openAiService;

        public TripPlanController(
            MongoDbService mongoDbService,
            IConfiguration configuration,
            IGooglePlacesService googlePlacesService,
            OpenAiService openAiService
        
        )
        {
            var tripPlanCollectionName = configuration.GetConnectionString("TripPlanCollection");
            _tripPlan = mongoDbService.Database.GetCollection<TripPlan>(tripPlanCollectionName);

            var userCollectionName = configuration.GetConnectionString("UserCollection");
            _user = mongoDbService.UserDatabase.GetCollection<User>(userCollectionName);

            _googlePlacesService = googlePlacesService;

            _openAiService = openAiService;
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

        [AllowAnonymous]
        [HttpGet("{tripId}/participants")]
        public async Task<ActionResult> Participants(string tripId)
        {

            List<User> users = new List<User>();

            var tripPlan = await _tripPlan.Find(t => t.Id == tripId).FirstOrDefaultAsync(); 

            foreach (var userId in tripPlan.Participants)
            {
                var user = await _user.Find(u => u.Id == userId).FirstOrDefaultAsync();
                users.Add(user);
            }

            return Ok(users);
        }

        [AllowAnonymous]
        [HttpPost("/getOpenAiResponse")]
        public async Task<ActionResult> GetOpenAiResponse([FromBody] OpenAIRequest request)
        {
            var response = await _openAiService.GetOpenAiResponse(request.text);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("/aiGenerateTripPlan/{userId}")]
        public async Task<ActionResult> AiGenerateTripPlan(string userId)
        {
            //674e42c8456b55bec98b2d78

            var random = new Random();
            var startDate = DateTime.Now.AddDays(random.Next(1, 30));
            var endDate = startDate.AddDays(random.Next(1, 30));
            var minAge = random.Next(18, 50);
            var maxAge = random.Next(minAge, 50);
            

            //pick one languageOption from ApiKeyAndUrl
            var languageOptions = ApiKeyAndUrl.LanguageOptions;



            List<string> pickLanguages = new List<string>();

            for (int i = 0; i < random.Next(languageOptions.Length); i++)
            {
                pickLanguages.Add(languageOptions[random.Next(languageOptions.Length)]);
            }

            List<CityPlan> cityPlans = new List<CityPlan>();
            
                var cityStartDate = DateTime.Now.AddDays(random.Next(1, 30));
                var cityEndDate = startDate.AddDays(random.Next(1, 30));
                var originLocation = await _openAiService.GetOpenAiResponse("write me only some random place name");
                var originLocationWitoutNull = originLocation == null ? "Tokyo" : originLocation;


                List<Accommodation> accommodations = new List<Accommodation>();
                

                    var locationAcc = await _openAiService.GetOpenAiResponse("write me only some random place name");
                    var ocationAccWitoutNull = locationAcc == null ? "Tokyo" : locationAcc;

                    var accomodation2 = new Accommodation()
                    {
                        location_acc = ocationAccWitoutNull,
                        name = await _openAiService.GetOpenAiResponse("pick one accomodation name in city" + originLocationWitoutNull),
                        description = await _openAiService.GetOpenAiResponse("write some brief description"),
                        image_url = "empty",
                        googleMapUrl = "empty"
                    };

                    accommodations.Add(accomodation2);
                

                List<Place> places = new List<Place>();
                

                    var loc = await _openAiService.GetOpenAiResponse("write me only some random place name");
                    var locWitoutNull = loc == null ? "Tokyo" : loc;

                    var place2 = new Place()
                    {
                        location = locWitoutNull,
                        details = await _openAiService.GetOpenAiResponse("Generate a brief description for a place in city" + originLocationWitoutNull),
                        image_url = "empty",
                        googleMapUrl = "empty"
                    };

                    places.Add(place2);
                

                var cityPlan = new CityPlan()
                {
                    StartDate = cityStartDate,
                    EndDate = cityEndDate,
                    OriginLocation = originLocationWitoutNull,
                    Image_url = "empty",
                    Transport = (Transport)random.Next(Enum.GetValues(typeof(Transport)).Length),
                    Accommodations = accommodations,
                    Places = places,
                    Description = await _openAiService.GetOpenAiResponse("Generate a brief description for a city")
                };



                cityPlans.Add(cityPlan);
            


            var tp = new TripPlan
            {
                UserId = userId,
                Participants = new List<string> { userId },
                Title = await _openAiService.GetOpenAiResponse("Generate a brief title for a trip to New York and Los Angeles"),
                Description = await _openAiService.GetOpenAiResponse("Generate a brief description for a trip max 40 words"),
                StartDate = startDate,
                EndDate = endDate,
                /*CityPlans = cityPlans,*/
                CityPlans = cityPlans,
                Languages = pickLanguages.ToArray(),
                Age = new Age { Min = minAge, Max = maxAge },
                GenderParticipants = (GenderParticipants)random.Next(Enum.GetValues(typeof(GenderParticipants)).Length),
                WithChildren = random.Next(2) == 0,
                Budget = random.Next(2, 10000),
                GroupType = random.Next(1, 20),
                TypeTravel = ApiKeyAndUrl.TravelTypesArray[random.Next(ApiKeyAndUrl.TravelTypesArray.Length)],
                ParticipantsFromOtherCountries = random.Next(2) == 0,
                OpenForBussines = false
            };

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

       

        
    }

}
