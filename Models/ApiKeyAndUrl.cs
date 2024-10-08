﻿namespace WayWIthUs_Server.Models
{
    public static class ApiKeyAndUrl
    {
        public static readonly string PLACES_API_KEY = "AIzaSyAaeF8LsmkU_Al-0NJ0Isg2BWvNOTrSxvY";
        public static readonly string GEMINI_API_KEY = "AIzaSyBlCJARSX2-4UG1iKEQW2cg29yYjjmJjc0";
        public static readonly string CLIEND_ID_OAUTH = "855465720082-amiqkfaec83mnj7ttmjagp3ueavhqqr7.apps.googleusercontent.com";
        public static readonly string USER_INFO = "https://www.googleapis.com/oauth2/v2/userinfo?access_token={token}";
        public static readonly string PLACES_API_TEXT_SEARCH = "https://places.googleapis.com/v1/places:searchText";
        public static readonly string PLACES_API_IMAGE_SEARCH = "https://places.googleapis.com/v1/{name}/media?key={apikey}&{params}";
    };
}
