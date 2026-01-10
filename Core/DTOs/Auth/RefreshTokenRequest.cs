using System.ComponentModel.DataAnnotations;
using Core.Constants;

namespace Core.DTOs.Auth;

public class RefreshTokenRequest
{
    [Required(ErrorMessage = ValidationMessages.RefreshTokenRequired)]
    public required string RefreshToken { get; set; }
}