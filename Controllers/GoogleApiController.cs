using GoogleApi.Entities.Maps.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WayWIthUs_Server.Models;
using WayWIthUs_Server.Service;

namespace WayWIthUs_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleApiController : ControllerBase
    {
        private readonly IGooglePlacesService _googlePlacesService;


        public GoogleApiController(IGooglePlacesService googlePlacesService) {
            _googlePlacesService = googlePlacesService;
        }



        [HttpGet("/details")]
        public async Task<PlacesResponse> GetDetails([FromQuery]string location)
        {
            return await _googlePlacesService.getPlacesDetails(location);
        }


        [HttpGet("/photos")]
        public async Task<List<string>> GetPhotos([FromQuery]string location, int height, int width)
        {
            return await _googlePlacesService.getPhotoUrls(location, height, width);
        }

    }
}
