namespace Core.Entities;

public class QuizQuestion : BaseEntity
{
    public int QuizId { get; set; }
    public required Quiz Quiz { get; set; }
    
    public int QuestionId { get; set; }
    public required Question Question { get; set; }

    public int Position { get; set; } // Za rangiranje pitanja
}