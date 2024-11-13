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

        public GeminiService() {
            _model = new GenerativeModel(ApiKeyAndUrl.GEMINI_API_KEY);
            _chat = _model.StartChat(new StartChatParams()); 
        }

        public async Task<string> OnMessage(string message){
            return await _chat.SendMessageAsync(message);
        }
    }
}


