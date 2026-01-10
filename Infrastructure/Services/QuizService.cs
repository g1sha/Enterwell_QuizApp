using Core.Constants;
using Core.DTOs.Pagination;
using Core.DTOs.Question;
using Core.DTOs.Quiz;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class QuizService(QuizContext context , IHttpContextAccessor httpContextAccessor) : IQuizService
{
    // Helper method to get the current user's ID from the HTTP context
    private string? GetCurrentUserId() => httpContextAccessor.HttpContext?.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
    
    // Retrieves a paginated list of quizzes
    public async Task<PaginatedResponseDto<QuizDto>> GetAllQuizzesAsync(int pageNumber = 1, int pageSize = 10)
    {
        return await context.Quizzes
            .Select(quiz => new QuizDto
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Description = quiz.Description,
            }).ToPaginatedAsync(pageNumber, pageSize);
    }

    // Retrieves a quiz by its ID, including its questions
    public async Task<QuizDetailDto?> GetQuizByIdAsync(int id)
    {
        return await context.Quizzes
            .Include(q =>q.QuizQuestions)
            .ThenInclude(qq=>qq.Question)
            .FirstOrDefaultAsync(q=>q.Id==id) is Quiz quiz
            ? new QuizDetailDto
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Description = quiz.Description,
                Questions = quiz.QuizQuestions.Select(q=> new QuestionDto
                {
                    Id = q.Question.Id,
                    Text = q.Question.Text,
                    CorrectAnswer =  q.Question.CorrectAnswer,
                }).ToList(),
            }
            : null;
    }

    // Creates a new quiz
    public async Task<QuizDto> CreateQuizAsync(CreateQuizDto quiz)
    {
        var newQuiz = new Quiz
        {
            Title = quiz.Title,
            Description = quiz.Description,
        };
        
        newQuiz.SetCreatedBy(GetCurrentUserId()??"Anonymous");
        
        context.Quizzes.Add(newQuiz);
        
        await context.SaveChangesAsync();
        
        return new QuizDto
        {
            Id = newQuiz.Id,
            Title = newQuiz.Title,
            Description = newQuiz.Description,
        };
    }

    // Updates an existing quiz
    public async Task<QuizDto?> UpdateQuizAsync(int id, UpdateQuizDto quiz)
    {
        var existingQuiz = await context.Quizzes.FindAsync(id);
        if (existingQuiz == null)
        {
            return null;
        }
        existingQuiz.Title = quiz.Title;
        existingQuiz.Description = quiz.Description;
        existingQuiz.MarkAsUpdated(GetCurrentUserId()??"Anonymous");
        
        await context.SaveChangesAsync();
        return new QuizDto
        {
            Id = existingQuiz.Id,
            Title = existingQuiz.Title,
            Description = existingQuiz.Description,
        };
    }
    
    // Deletes a quiz by its ID
    public async Task<bool> DeleteQuizAsync(int id)
    {
        var existingQuiz = await context.Quizzes.FindAsync(id);
        if (existingQuiz != null)
        {
            context.Quizzes.Remove(existingQuiz);
            await context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    // Adds a question to a quiz
    public async Task<(bool Success, string? Error)> AddQuestionToQuizAsync(int quizId, AddQuestionToQuizDto dto)
    {
        var quiz = await context.Quizzes.FindAsync(quizId);
        var question = await context.Questions.FindAsync(dto.QuestionId);
        
        if (quiz == null || question == null)
            return (false, ErrorMessages.QuizOrQuestionNotFound);

        var alreadyExists = await context.QuizQuestions
            .AnyAsync(q => q.QuestionId == dto.QuestionId && q.QuizId == quizId);
        
        if (alreadyExists)
            return (false, ErrorMessages.QuestionAlreadyInQuiz);
                
        var quizQuestion = new QuizQuestion
        {
            Quiz = quiz,
            Question = question,
        };
        quizQuestion.SetCreatedBy(GetCurrentUserId()??"Anonymous");
        
        context.QuizQuestions.Add(quizQuestion);
        await context.SaveChangesAsync();
        return (true, null);
    }

    // Removes a question from a quiz without deleting the question itself
    public async Task<(bool Success, string? Error)> RemoveQuestionFromQuizAsync(int quizId, AddQuestionToQuizDto dto)
    {
        var quizQuestion = await context.QuizQuestions
            .FirstOrDefaultAsync(q => q.QuestionId == dto.QuestionId && q.QuizId == quizId);
        if (quizQuestion == null)
            return (false, ErrorMessages.QuizOrQuestionNotFound);
        context.QuizQuestions.Remove(quizQuestion);
        await context.SaveChangesAsync();
        return (true, null);
    }
}