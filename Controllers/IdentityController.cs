using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GoogleApi.Entities.Maps.AerialView.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using WayWIthUs_Server.Data;
using WayWIthUs_Server.Entities;
using WayWIthUs_Server.Models;

namespace WayWIthUs_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private static readonly TimeSpan TokenLifeTime = TimeSpan.FromHours(1);
        private IConfiguration _configuration;
        private readonly IMongoCollection<User> _user;

        public IdentityController(MongoDbService mongoDbService,IConfiguration configuration)
        {
            _configuration = configuration;
            var userdbCollectionName = configuration.GetConnectionString("UserCollection");
            _user = mongoDbService.UserDatabase.GetCollection<User>(userdbCollectionName);

        }

        [HttpPost("login")]
        public IActionResult GenerateToken([FromBody] TokenGenerationRequest request){

            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Username and password are required.");
            }

            //find user in database
            var user = _user.Find(u => u.email == request.Email).FirstOrDefault();
            if (user == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            /*var passwordHash = BCrypt.Net.BCrypt.HashPassword("1234");*/
            var passwordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.password);
            if(!passwordValid)
            {
                return Unauthorized("Password is incorrect.");
            }

            var Issuer = _configuration["JwtSettings:Issuer"];
            var Audience = _configuration["JwtSettings:Audience"];
            var key = _configuration["JwtSettings:Key"];
            var TokenValidInMinutes = _configuration.GetValue<int>("JwtSettings:TokenValidInMinutes");
            var TokenExpiryTimeSpan = DateTime.UtcNow.AddMinutes(TokenValidInMinutes);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Name, request.Email)
                }),
                Expires = TokenExpiryTimeSpan,
                Issuer = Issuer,
                Audience = Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler(); 
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            if (token == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            return Ok(new LoginResponse(){ 
                User = user,
                AccessToken = jwt,
                ExpiresIn = (int)TokenLifeTime.TotalSeconds
             });
        }
    }
}
