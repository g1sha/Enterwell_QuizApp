namespace Core.DTOs.Auth;

public class AuthResult
{
    public bool IsSuccessful { get; init; }
    public AuthTokenDto? Tokens { get; init; }
    public List<string> Errors { get; init; } = [];

    public static AuthResult Success(string token, string refreshToken) => new()
    {
        IsSuccessful = true,
        Tokens = new AuthTokenDto { Token = token, RefreshToken = refreshToken }
    };

    public static AuthResult Success() => new() { IsSuccessful = true };

    public static AuthResult Failure(string error) => new()
    {
        IsSuccessful = false,
        Errors = [error]
    };

    public static AuthResult Failure(List<string> errors) => new()
    {
        IsSuccessful = false,
        Errors = errors
    };

    public static AuthResult Failure(IEnumerable<string> errors) => new()
    {
        IsSuccessful = false,
        Errors = errors.ToList()
    };
}

public class AuthTokenDto
{
    public string Token { get; init; } = string.Empty;
    public string RefreshToken { get; init; } = string.Empty;
}