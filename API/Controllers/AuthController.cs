using Core.DTOs;
using Core.DTOs.Auth;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthController(AuthService authService) : ControllerBase
{
    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterUserDto dto)
    {
        var result=await authService.RegisterAsync(dto);
        if (!result.IsSuccessful)
            return BadRequest(ErrorResponse.Create(result.Errors));
        return Ok(new { message = "Registration successful !" });
    }

    /// <summary>
    /// Login an existing user
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginUserDto dto)
    {
        var result = await authService.LoginAsync(dto);
        if (!result.IsSuccessful)
            return BadRequest(ErrorResponse.Create(result.Errors));
        return Ok(result.Tokens);
    }
    
    /// <summary>
    /// Retrieve new JWT token using refresh token
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("refresh")]
    public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var result = await authService.RefreshTokenAsync(request.RefreshToken);
        if (!result.IsSuccessful)
            return Unauthorized(ErrorResponse.Create(result.Errors));
        return Ok(result.Tokens);
    }
}