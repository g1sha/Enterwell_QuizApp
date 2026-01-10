using Core.DTOs.Pagination;
using Core.DTOs.Question;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class QuestionService(QuizContext context) : IQuestionService
{
    public async Task<PaginatedResponseDto<QuestionDto>> GetAllQuestionsAsync(int pageNumber = 1, int pageSize = 10, string ?filter = null)
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

    public async Task<QuestionDto> CreateQuestionAsync(CreateQuestionDto question)
    {
        var newQuestion = new Question
        {
            Text = question.Text,
            CorrectAnswer = question.CorrectAnswer,
        };
    
        context.Questions.Add(newQuestion);
        await context.SaveChangesAsync();
    
        return new QuestionDto
        {
            Id = newQuestion.Id,
            Text = newQuestion.Text,
            CorrectAnswer = newQuestion.CorrectAnswer,
        };
    }

    public async Task<QuestionDto?> UpdateQuestionAsync(int id, UpdateQuestionDto question)
    {
        var existingQuestion = await context.Questions.FirstOrDefaultAsync(q => q.Id == id);
        if (existingQuestion == null)
        {
            return null;
        }

        existingQuestion.Text = question.Text; 
        existingQuestion.CorrectAnswer = question.CorrectAnswer;
        existingQuestion.MarkAsUpdated();

        await context.SaveChangesAsync();
        
        return new QuestionDto()
        {
            Id = existingQuestion.Id,
            Text = existingQuestion.Text,
            CorrectAnswer = existingQuestion.CorrectAnswer,
        };
    }
}

