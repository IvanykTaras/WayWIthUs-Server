using System;
using OpenAI.Chat;
using WayWIthUs_Server.Models;

namespace WayWIthUs_Server.Service;

public class OpenAiService
{
    private readonly ChatClient client;

    public OpenAiService()
    {
        client = new(model: "gpt-4o-mini", apiKey: ApiKeyAndUrl.OPEN_KEY);
    }

    public async Task<string> GetOpenAiResponse(string text)
    {
        ChatCompletion completion = await client.CompleteChatAsync(text);
        string Content = completion.Content[0].Text;
        Console.WriteLine(Content);
        return Content;
    }

}
