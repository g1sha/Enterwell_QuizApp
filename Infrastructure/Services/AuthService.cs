using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Core.DTOs.Auth;
using Core.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

public class AuthService(UserManager<User> userManager, IConfiguration configuration)
{
    public async Task<AuthResult> RegisterAsync(RegisterUserDto dto)
    {
        var user = new User
        {
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName =  dto.LastName,
            UserName = dto.Email,
            RefreshToken = GenerateRefreshToken(),
            RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7)
        };
        var result = await userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return new AuthResult { IsSuccessful = false, Errors = result.Errors.Select(e => e.Description).ToList() };
        
        return new AuthResult { IsSuccessful = true };
    }
    
    public async Task<AuthResult> LoginAsync(LoginUserDto dto)
    {
        var user = await userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            return new AuthResult{IsSuccessful = false, Errors = new List<string>{"Invalid email."}};
        
        var passwordValid = await userManager.CheckPasswordAsync(user, dto.Password);
        if (!passwordValid)
            return new AuthResult{IsSuccessful = false, Errors = new List<string>{"Invalid password."}};

        user.LastLoginAt= DateTime.UtcNow;
        user.RefreshToken = GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await userManager.UpdateAsync(user);
        
        var token = await GenerateJwtToken(user);
        return new AuthResult{IsSuccessful = true, Token = $"Bearer {token}", RefreshToken = user.RefreshToken};
    }
    
    private async Task<string> GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.NameId, $"{user.FirstName} {user.LastName}"),
        };
        
        var roles = await userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(configuration["Jwt:DurationInMinutes"])),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[512];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
    
    public async Task<AuthResult> RefreshTokenAsync(string refreshToken)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            return new AuthResult { IsSuccessful = false, Errors = new List<string> { "Invalid or expired refresh token !" } };

        var token = await GenerateJwtToken(user);
        
        //Rotate refresh token
        user.RefreshToken = GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await userManager.UpdateAsync(user);
        
        return new AuthResult { IsSuccessful = true, Token = $"Bearer {token}", RefreshToken = user.RefreshToken };
    }
}