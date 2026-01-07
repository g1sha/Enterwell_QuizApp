namespace Core.Entities;

public class Question : BaseEntity
{
    public required string Text { get; set; }
    public required string CorrectAnswer { get; set; }
    
    public ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();
}