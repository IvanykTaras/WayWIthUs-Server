using System;

namespace WayWIthUs_Server.Models;

public class LoginResponse
{
    public string UserName { get; set; }
    public string AccessToken { get; set; }
    public int ExpiresIn { get; set; } // in seconds
}
