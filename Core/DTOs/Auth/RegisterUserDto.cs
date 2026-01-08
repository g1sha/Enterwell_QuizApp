using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.Auth;

public class RegisterUserDto
{
    [Required, EmailAddress]
    public required string Email { get; set; }
    [Required, MinLength(6)]
    public required string Password { get; set; }
    [Required, MaxLength(35)]
    public required string FirstName { get; set; }
    [Required, MaxLength(35)]
    public required string LastName { get; set; }
}

public class RegisteredUserDto
{
    public required string Id { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}