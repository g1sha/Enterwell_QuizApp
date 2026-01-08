using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Core.DTOs.Auth;
using Core.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
            UserName = dto.Email
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

        
        var token = GenerateJwtToken(user);
        return new AuthResult{IsSuccessful = true, Token = $"Bearer {await token}"};
    }
    
    private async Task<string> GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
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
}