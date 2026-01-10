﻿using System.ComponentModel.DataAnnotations;
using Core.Constants;

namespace Core.DTOs.Auth;

public class LoginUserDto
{
    [Required(ErrorMessage = ValidationMessages.EmailRequired)]
    [EmailAddress(ErrorMessage = ValidationMessages.EmailInvalid)]
    public required string Email { get; set; }
    
    [Required(ErrorMessage = ValidationMessages.PasswordRequired)]
    public required string Password { get; set; }
}