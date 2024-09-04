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
    public class CustomerController : ControllerBase
    {
        private readonly IMongoCollection<Customer>? _customers; 
        public CustomerController(MongoDbService mongoDbService) {
            _customers = mongoDbService.Database.GetCollection<Customer>("customer");
        }

        [HttpGet]
        public async Task<List<Customer>> Get()
        {
            return await _customers.Find(FilterDefinition<Customer>.Empty).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<Customer> GetById(ObjectId id)
        {
            var filter = Builders<Customer>.Filter.Eq(e => e.Id,id);
            return await _customers.Find(filter).FirstOrDefaultAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post(Customer customer)
        {
            await _customers.InsertOneAsync(customer);
            return CreatedAtAction(nameof(GetById),new {id = customer.Id},customer);
        }
    }
}
