using System.ComponentModel.DataAnnotations;
using Core.Constants;

namespace Core.DTOs.Auth;

public class RegisterUserDto
{
    [Required(ErrorMessage = ValidationMessages.EmailRequired)]
    [EmailAddress(ErrorMessage = ValidationMessages.EmailInvalid)]
    public required string Email { get; set; }
    
    [Required(ErrorMessage = ValidationMessages.PasswordRequired)]
    [MinLength(6, ErrorMessage = ValidationMessages.PasswordLength)]
    public required string Password { get; set; }
    
    [Required(ErrorMessage = ValidationMessages.FirstNameRequired)]
    [MaxLength(35)]
    public required string FirstName { get; set; }
    
    [Required(ErrorMessage = ValidationMessages.LastNameRequired)]
    [MaxLength(35)]
    public required string LastName { get; set; }
}

public class RegisteredUserDto
{
    public required string Id { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}