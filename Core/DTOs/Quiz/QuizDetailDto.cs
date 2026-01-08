using Core.DTOs.Question;

namespace Core.DTOs.Quiz;

public class QuizDetailDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public List<QuestionDto> Questions { get; set; } = new();
}
