using System.ComponentModel.DataAnnotations;
using Core.Constants;

namespace Core.DTOs.Quiz;

public class UpdateQuizDto
{
    [Required(ErrorMessage = ValidationMessages.QuizTitleRequired)]
    [StringLength(500, MinimumLength = 3, ErrorMessage = ValidationMessages.QuizTitleLength)]
    public string Title { get; set; }

    [StringLength(1000, ErrorMessage = ValidationMessages.QuizDescriptionLength)]
    public string Description { get; set; } = string.Empty;
}

