using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Driver.Core.Connections;
using WayWIthUs_Server.Data;
using WayWIthUs_Server.Entities;

namespace WayWIthUs_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : Controller
    {
        private readonly IMongoCollection<Message> _message;

        public MessageController(MongoDbService mongoDbService, IConfiguration configuration)
        {
            
            var messageCollection = configuration.GetConnectionString("MessageCollection");
            _message = mongoDbService.Database.GetCollection<Message>(messageCollection);
        }

        [HttpGet]
        public async Task<List<Message>> Get() => await _message.Find(FilterDefinition<Message>.Empty).ToListAsync();

        [HttpGet("{tripId}")]
        public async Task<List<Message>> Get(string tripId) => await _message.Find(x => x.TripId == tripId).ToListAsync();

        [HttpDelete("{id}")]
        public async Task Delete(string id) => await _message.DeleteOneAsync(x => x.Id == id);
        [HttpDelete("/deleteAll")]
        public async Task DeleteAll() => await _message.DeleteManyAsync(FilterDefinition<Message>.Empty);

        [HttpPost]
        public async Task Create(Message message) => await _message.InsertOneAsync(message);


    }
}
