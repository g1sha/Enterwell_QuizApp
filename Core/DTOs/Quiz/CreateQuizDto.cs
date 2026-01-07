using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.Quiz;

public class CreateQuizDto
{
    [Required(ErrorMessage = "Title of quiz is required !")]
    [StringLength(500, MinimumLength = 5, ErrorMessage = "Quiz tuitle must be between 5 and 500 characters !")]
    public string Title { get; set; }

    [StringLength(1000, ErrorMessage = "Description text cannot be more than 1000 characters !")]
    public string Description { get; set; } = string.Empty;
}

