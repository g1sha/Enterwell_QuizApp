namespace Core.DTOs.Auth;

public class AuthResult
{
    public bool IsSuccessful { get; set; }
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public List<string> Errors { get; set; } = new();
}