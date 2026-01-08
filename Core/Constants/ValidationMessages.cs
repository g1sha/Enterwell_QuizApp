namespace Core.Constants;

public class ValidationMessages
{
    // Question DTO
    public const string QuestionTextRequired = "Question text is required !";
    public const string QuestionTextLength = "Question must be between 10 and 500 characters !";
    public const string QuestionCorrectAnswerRequired = "Correct answer is required !";
    public const string QuestionCorrectAnswerLength = "Question must be between 10 and 500 characters !";
    
    //Quiz DTO
    public const string QuizTitleRequired = "Title of quiz is required !";
    public const string QuizTitleLength = "Quiz title must be between 5 and 500 characters !";
    public const string QuizDescriptionLength = "Description text cannot be more than 1000 characters !";
}