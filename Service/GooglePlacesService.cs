using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using WayWIthUs_Server.Entities;
using WayWIthUs_Server.Models;

namespace WayWIthUs_Server.Service
{
    public class GooglePlacesService: IGooglePlacesService
    {

        public async Task<PlacesResponse> getPlacesDetails(string location)
        {
            HttpClient client = new();
            var request = new HttpRequestMessage(HttpMethod.Post, ApiKeyAndUrl.PLACES_API_TEXT_SEARCH);
            request.Headers.Add("X-Goog-Api-Key", ApiKeyAndUrl.PLACES_API_KEY);
            request.Headers.Add("X-Goog-FieldMask", "places.photos,places.googleMapsUri,places.displayName");

            var jsonContent = JsonConvert.SerializeObject(new
            {
                textQuery = location
            });

            request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");


            var response = await client.SendAsync(request);
            
            return await response.Content.ReadFromJsonAsync<PlacesResponse>();

        }

        public async Task<List<string>> getPhotoUrls(string location, int height, int width) {
            List<string> photoName = (await getPlacesDetails(location))
                .Places
                .FirstOrDefault()
                .Photos
                .Select(e => {
                    return ApiKeyAndUrl.PLACES_API_IMAGE_SEARCH
                    .Replace("{name}", e.Name)
                    .Replace("{apikey}", ApiKeyAndUrl.PLACES_API_KEY)
                    .Replace("{params}", $"maxHeightPx={height}&maxWidthPx={width}");
                })
                .ToList();
            return photoName;
        }
    }
}
