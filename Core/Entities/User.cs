using Microsoft.AspNetCore.Identity;

namespace Core.Entities;

public class User:IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastLoginAt { get; set; }
    // public string RefreshToken { get; set; }
    // public DateTime RefreshTokenExpiryTime { get; set; }

    public User()
    {
        CreatedAt = DateTime.Now;
    }
}