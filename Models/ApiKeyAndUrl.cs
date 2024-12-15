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

        public static readonly string[] RandomCities = new string[]
        {
            "New York", "Los Angeles", "Chicago", "Houston", "Phoenix", "Philadelphia", "San Antonio", "San Diego", "Dallas", "San Jose",
            "Austin", "Jacksonville", "Fort Worth", "Columbus", "Charlotte", "San Francisco", "Indianapolis", "Seattle", "Denver", "Washington",
            "Boston", "El Paso", "Nashville", "Detroit", "Oklahoma City", "Portland", "Las Vegas", "Memphis", "Louisville", "Baltimore",
            "Milwaukee", "Albuquerque", "Tucson", "Fresno", "Sacramento", "Kansas City", "Long Beach", "Mesa", "Atlanta", "Colorado Springs",
            "Virginia Beach", "Raleigh", "Omaha", "Miami", "Oakland", "Minneapolis", "Tulsa", "Wichita", "New Orleans", "Arlington",
            "Cleveland", "Bakersfield", "Tampa", "Aurora", "Honolulu", "Anaheim", "Santa Ana", "Corpus Christi", "Riverside", "Lexington",
            "St. Louis", "Stockton", "Pittsburgh", "Saint Paul", "Cincinnati", "Anchorage", "Henderson", "Greensboro", "Plano", "Newark",
            "Lincoln", "Toledo", "Orlando", "Chula Vista", "Irvine", "Fort Wayne", "Jersey City", "Durham", "St. Petersburg", "Laredo",
            "Buffalo", "Madison", "Lubbock", "Chandler", "Scottsdale", "Glendale", "Reno", "Norfolk", "Winston-Salem", "North Las Vegas",
            "Irving", "Chesapeake", "Gilbert", "Hialeah", "Garland", "Fremont", "Richmond", "Boise", "Baton Rouge", "Des Moines"
        };
    }
}
