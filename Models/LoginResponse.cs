using System;
using WayWIthUs_Server.Entities;

namespace WayWIthUs_Server.Models;

public class LoginResponse
{
    public User User { get; set; }
    public string AccessToken { get; set; }
    public int ExpiresIn { get; set; } // in seconds
}
