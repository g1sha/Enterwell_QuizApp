using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.Quiz;

public class UpdateQuizDto
{
    [Required(ErrorMessage = "Title is required !")]
    [StringLength(500, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 500 characters !")]
    public string Title { get; set; }

    [StringLength(1000, ErrorMessage = "Description text cannot be more than 1000 characters !")]
    public string Description { get; set; } = string.Empty;
}

