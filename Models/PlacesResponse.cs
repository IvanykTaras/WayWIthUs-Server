namespace WayWIthUs_Server.Models
{
    public class PlacesResponse
    {
        public List<PlaceResponse> Places { get; set; }
    }

    public class PlaceResponse
    {
        public string GoogleMapsUri { get; set; }
        public DisplayNameResponse DisplayName { get; set; }
        public List<PhotoResponse> Photos { get; set; }
    }

    public class DisplayNameResponse
    {
        public string Text { get; set; }
        public string LanguageCode { get; set; }
    }

    public class PhotoResponse
    {
        public string Name { get; set; }
        public int WidthPx { get; set; }
        public int HeightPx { get; set; }
        public List<AuthorAttributionResponse> AuthorAttributions { get; set; }
    }

    public class AuthorAttributionResponse
    {
        public string DisplayName { get; set; }
        public string Uri { get; set; }
        public string PhotoUri { get; set; }
    }
}
