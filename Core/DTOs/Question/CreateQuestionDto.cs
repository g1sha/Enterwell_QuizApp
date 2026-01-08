using System.ComponentModel.DataAnnotations;
using Core.Constants;

namespace Core.DTOs.Question;

public class CreateQuestionDto
{
    [Required(ErrorMessage = ValidationMessages.QuestionTextRequired)]
    [StringLength(500, MinimumLength = 10, ErrorMessage = ValidationMessages.QuestionTextLength)]
    public string Text { get; set; }

    [Required(ErrorMessage = ValidationMessages.QuestionCorrectAnswerRequired)]
    [StringLength(500, MinimumLength = 1, ErrorMessage = ValidationMessages.QuestionCorrectAnswerLength)]
    public string CorrectAnswer { get; set; }
}

