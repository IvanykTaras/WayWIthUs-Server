using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using WayWIthUs_Server.Data;
using WayWIthUs_Server.Entities;

namespace WayWIthUs_Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMongoCollection<User> _user;
        public UserController(MongoDbService mongoDbService, IConfiguration configuration)
        {
            var userdbCollectionName = configuration.GetConnectionString("UserCollection");
            _user = mongoDbService.UserDatabase.GetCollection<User>(userdbCollectionName);   
        }

        [HttpGet]
        public async Task<List<User>> Get()
        {
            return await _user.Find(FilterDefinition<User>.Empty).ToListAsync();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Post(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _user.Find(u => u.email == user.email).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                return BadRequest("User with this email already exists.");
            }
 
            string newPassword = BCrypt.Net.BCrypt.HashPassword(user.password);
            user.password = newPassword;       
            await _user.InsertOneAsync(user);
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, User updatedUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _user.Find(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }

            await _user.ReplaceOneAsync(u => u.Id == id, updatedUser);
            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _user.Find(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }

            await _user.DeleteOneAsync(u => u.Id == id);
            return NoContent();
        }



    }
}
