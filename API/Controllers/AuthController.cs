
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Core.DTOs.Auth;
using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthController(AuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterUserDto dto)
    {
        var result=await authService.RegisterAsync(dto);
        if (!result.IsSuccessful)
            return BadRequest(new { result.Errors});
        return Ok(new { result});
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginUserDto dto)
    {
        var result = await authService.LoginAsync(dto);
        if (!result.IsSuccessful)
            return BadRequest(new { result.Errors});
        return Ok(new { result.Token });
}
}