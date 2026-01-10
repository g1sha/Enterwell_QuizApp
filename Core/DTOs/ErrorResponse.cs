namespace Core.DTOs;

public class ErrorResponse
{
    public List<string> Errors { get; init; } = [];
    private ErrorResponse() { }
    public static ErrorResponse Create(string error) => new() { Errors = [error] };
    public static ErrorResponse Create(List<string> errors) => new() { Errors = errors };
    public static ErrorResponse Create(IEnumerable<string> errors) => new() { Errors = errors.ToList() };
}

