using WayWIthUs_Server.Models;

namespace WayWIthUs_Server.Service
{
    public interface IGooglePlacesService
    {
        Task<PlacesResponse> getPlacesDetails(string data);
        Task<List<string>> getPhotoUrls(string location, int height, int width);

        Task<string> getPlaceLink(string location);
    }
}
