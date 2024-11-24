namespace WayWIthUs_Server.Models
{
    public static class ApiKeyAndUrl
    {
        public static readonly string PLACES_API_KEY = "AIzaSyBPcSKuwLsx_Jr4npHMzWpKFhupBms7xfo";
        public static readonly string GEMINI_API_KEY = "AIzaSyBlCJARSX2-4UG1iKEQW2cg29yYjjmJjc0";
        public static readonly string CLIEND_ID_OAUTH = "367709085945-mlgcuaadpsp0fmc5fqv955mo09jjg11f.apps.googleusercontent.com";
        public static readonly string USER_INFO = "https://www.googleapis.com/oauth2/v2/userinfo?access_token={token}";
        public static readonly string PLACES_API_TEXT_SEARCH = "https://places.googleapis.com/v1/places:searchText";
        public static readonly string PLACES_API_IMAGE_SEARCH = "https://places.googleapis.com/v1/{name}/media?key={apikey}&{params}";
    };
}
