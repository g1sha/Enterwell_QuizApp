using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.Auth;

public class LoginUserDto
{
    [Required, EmailAddress]
    public required string Email { get; set; }
    [Required]
    public required string Password { get; set; }
}