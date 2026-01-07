namespace Core.Entities;

public class Quiz : BaseEntity
{
    public required string Title { get; set; }
    public string? Description { get; set; }

    public ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();
}