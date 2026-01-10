using Core.DTOs.Pagination;
using Core.DTOs.Question;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class QuestionService(QuizContext context, IHttpContextAccessor httpContextAccessor) : IQuestionService
{
    // Helper method to get the current user's ID from the HTTP context
    private string? GetCurrentUserId() => httpContextAccessor.HttpContext?.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
    
    // Retrieves a paginated list of questions, optionally filtered by text
    public async Task<PaginatedResponseDto<QuestionDto>> GetAllQuestionsAsync(int pageNumber = 1, int pageSize = 10,
        string? filter = null)
    {
        var query = context.Questions.AsQueryable();

        if (!string.IsNullOrEmpty(filter))
        {
            query = query.Where(q => q.Text.Contains(filter));
        }

        return await query.Select(q => new QuestionDto
        {
            Id = q.Id,
            Text = q.Text,
            CorrectAnswer = q.CorrectAnswer,
        }).ToPaginatedAsync(pageNumber, pageSize);
    }

    // Retrieves a question by its ID
    public async Task<QuestionDto?> GetQuestionByIdAsync(int id)
    {
        return await context.Questions.FindAsync(id) is Question question
            ? new QuestionDto
            {
                Id = question.Id,
                Text = question.Text,
                CorrectAnswer = question.CorrectAnswer,
            }
            : null;
    }

    // Creates a new question
    public async Task<QuestionDto> CreateQuestionAsync(CreateQuestionDto question)
    {
        var newQuestion = new Question
        {
            Text = question.Text,
            CorrectAnswer = question.CorrectAnswer,
        };
        newQuestion.SetCreatedBy(GetCurrentUserId()??"Anonymous");

        context.Questions.Add(newQuestion);
        await context.SaveChangesAsync();

        return new QuestionDto
        {
            Id = newQuestion.Id,
            Text = newQuestion.Text,
            CorrectAnswer = newQuestion.CorrectAnswer,
        };
    }

    // Updates an existing question
    public async Task<QuestionDto?> UpdateQuestionAsync(int id, UpdateQuestionDto question)
    {
        var existingQuestion = await context.Questions.FirstOrDefaultAsync(q => q.Id == id);
        if (existingQuestion == null)
            return null;

        existingQuestion.Text = question.Text;
        existingQuestion.CorrectAnswer = question.CorrectAnswer;
        existingQuestion.MarkAsUpdated(GetCurrentUserId()??"Anonymous");

        await context.SaveChangesAsync();

        return new QuestionDto()
        {
            Id = existingQuestion.Id,
            Text = existingQuestion.Text,
            CorrectAnswer = existingQuestion.CorrectAnswer,
        };
    }
}

