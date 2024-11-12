using System;
using Newtonsoft.Json;
using GenerativeAI.Methods;
using GenerativeAI.Models;
using GenerativeAI.Types;
using WayWIthUs_Server.Models;

namespace WayWIthUs_Server.Service{
    public class GeminiService: IGeminiService
    {
        private GenerativeModel _model;
        private ChatSession _chat;

        private string aiPrompt = "Generate Travel Plan for Location: {location}, for {days_number} Days for {group_enum} with a {budget_enum} budget," +
                       "Give me a Hotels options list with HotelName, Hotel address, Price, hotel image url, geo coordinates, rating, descriptions and suggest itinerary with placeName, Place Details, Place Image Url, Geo Coordinates, ticket Pricing, rating, Time travel each of the location for 3 days with each day plan with best time to visit in JSON format. json format sample" +
                       "{" +
                       "    hotels: Array<{" +
                       "        name: string," +
                       "        address: string," +
                       "        price: string," +
                       "        image_url: string," +
                       "        geo_coordinates: string," +
                       "        rating: string," +
                       "        description:string" +
                       "    }>," +
                       "    itinerary: Array<{" +
                       "        day: number," +
                       "        places: Array<{" +
                       "            time: string," +
                       "            location: string," +
                       "            details: string," +
                       "            image_url: string," +
                       "            geo_coordinates: string," +
                       "            ticket_pricing: string," +
                       "            rating: string" +
                       "        }>" +
                       "    }>" +
                       "}";

        public GeminiService() {
            _model = new GenerativeModel(ApiKeyAndUrl.GEMINI_API_KEY);
            _chat = _model.StartChat(new StartChatParams(){
                GenerationConfig = new GenerationConfig(){
                    Temperature =  1,
                    TopP = 0.95,
                    TopK = 64,
                    MaxOutputTokens = 8192,
                }
            }); 
        }

        public async Task<string> GenerateTripJson(GeminiTrip trip){
            string finalPrompt = aiPrompt
                .Replace("{location}", trip.Location)
                .Replace("{days_number}", trip.DaysNumber)
                .Replace("{group_enum}", trip.GroupType.ToString())
                .Replace("{budget_enum}", trip.BudgetType.ToString());

            return await _chat.SendMessageAsync(finalPrompt);
        }
    }

    public class GeminiTrip
    {
        public string UserEmail { get; set; }
        public string Location { get; set; }
        public string DaysNumber { get; set; }
        public BudgetType BudgetType { get; set; }
        public GroupType GroupType { get; set; }
    }

    public enum BudgetType
    {
        Cheap,
        Moderate,
        Luxury
    }

    public enum GroupType
    {
        OnePerson,
        Couple,
        Family,
        Friends
    }
}


