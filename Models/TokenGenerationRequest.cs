using System;

namespace WayWIthUs_Server.Models;

public class TokenGenerationRequest
{
    public string Email { get; set; }
    public string? Password { get; set; }
}
