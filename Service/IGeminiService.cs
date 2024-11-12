using System;

namespace WayWIthUs_Server.Service{
    
    public interface IGeminiService
    {
        Task<string> GenerateTripJson(GeminiTrip trip);
    }
};

