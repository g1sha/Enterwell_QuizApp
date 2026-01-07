using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.Question;

public class QuestionDto
{
    public int Id { get; set; }
    public string Text { get; set; } 
    public string CorrectAnswer { get; set; } 
}

