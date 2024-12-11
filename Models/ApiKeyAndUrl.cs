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
        public static readonly string OPEN_KEY = "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";

        public static readonly string[] LanguageOptions = new string[]
        {
            "en", "pl", "af", "am", "ar", "az", "bg", "bn", "cs", "cy", "da", "de", "el", "eo", "es", "et", "fa", "fi", "fr", "ga", "gu", "he", "hi", "hr", "hu", "hy", "is", "it", "ja", "ka", "kk", "km", "kn", "ko", "la", "lo", "lt", "lv", "mk", "ml", "mn", "mr", "mt", "my", "ne", "nl", "no", "or", "pa", "pt", "ro", "ru", "si", "sk", "sl", "so", "sq", "sr", "sv", "sw", "ta", "te", "th", "tr", "uk", "ur", "uz", "vi", "xh", "yi", "zh", "zu"
        };

        public static readonly string[] TravelTypesArray = new string[]
        {
            "active",
            "recreational",
            "cultural",
            "sports_extreme",
            "automobile",
            "bicycle",
            "equestrian",
            "ski",
            "pilgrimage",
            "adventure",
            "water",
            "rural",
            "shopping",
            "passive",
            "health",
            "business",
            "gastronomic",
            "cruising",
            "educational"
        };
    };
}
