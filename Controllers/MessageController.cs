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
    }
}
