using Core.DTOs.Question;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class QuestionService(QuizContext context) : IQuestionService
{
    public async Task<IEnumerable<QuestionDto>> GetAllQuestionsAsync()
    {
        return await context.Questions.Select(q => new QuestionDto
        {
            Id = q.Id,
            Text = q.Text,
            CorrectAnswer = q.CorrectAnswer,
        }).ToListAsync<QuestionDto>();
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
        var existingQuestion = await context.Questions.FindAsync(id);
        if (existingQuestion == null)
        {
            return null;
        }

        existingQuestion.Text = question.Text;
        existingQuestion.CorrectAnswer = question.CorrectAnswer;
        existingQuestion.UpdatedAt = DateTime.Now;

        await context.SaveChangesAsync();
        
        return new QuestionDto()
        {
            Id = existingQuestion.Id,
            Text = existingQuestion.Text,
            CorrectAnswer = existingQuestion.CorrectAnswer,
        };
    }
}

