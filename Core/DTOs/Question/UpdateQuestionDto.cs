using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.Question;

public class UpdateQuestionDto
{
    [Required(ErrorMessage = "Question title is required !")]
    [StringLength(500, MinimumLength = 10, ErrorMessage = "Question must be between 10 and 500 characters !")]
    public string Text { get; set; }

    [Required(ErrorMessage = "Correct answer is required !")]
    [StringLength(500, MinimumLength = 1, ErrorMessage = "Question must be between 1 and 500 characters !")]
    public string CorrectAnswer { get; set; }
}

