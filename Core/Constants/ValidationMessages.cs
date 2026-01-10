namespace Core.Constants;

public static class ValidationMessages
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
    
    // Auth
    public const string EmailRequired = "Email is required !";
    public const string EmailInvalid = "Invalid email format !";
    public const string PasswordRequired = "Password is required !";
    public const string PasswordLength = "Password must be at least 6 characters !";
    public const string FirstNameRequired = "First name is required !";
    public const string LastNameRequired = "Last name is required !";
    public const string RefreshTokenRequired = "Refresh token is required !";
}

public static class ErrorMessages
{
    // Auth
    public const string InvalidCredentials = "Invalid email or password !";
    public const string InvalidOrExpiredRefreshToken = "Invalid or expired refresh token !";
    
    // Quiz
    public const string QuizNotFound = "Quiz not found !";
    public const string QuizOrQuestionNotFound = "Quiz or question not found !";
    public const string QuestionAlreadyInQuiz = "This question is already added to the quiz !";
    
    // Question
    public const string QuestionNotFound = "Question not found !";
}