using Core.DTOs.Quiz;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class QuizService(QuizContext context) : IQuizService
{
    public async Task<IEnumerable<QuizDto>> GetAllQuizzesAsync()
    {
        return await context.Quizzes.Select(quiz => new QuizDto
        {
            Id = quiz.Id,
            Title = quiz.Title,
            Description = quiz.Description,
        }).ToListAsync();
    }

    public async Task<QuizDto?> GetQuizByIdAsync(int id)
    {
        return await context.Quizzes.FindAsync(id) is Quiz quiz
            ? new QuizDto
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Description = quiz.Description,
            }
            : null;
    }

    public async Task<QuizDto> CreateQuizAsync(CreateQuizDto quiz)
    {
        var newQuiz = new Quiz
        {
            Title = quiz.Title,
            Description = quiz.Description,
        };
        
        context.Quizzes.Add(newQuiz);
        await context.SaveChangesAsync();
        
        return new QuizDto
        {
            Id = newQuiz.Id,
            Title = newQuiz.Title,
            Description = newQuiz.Description,
        };
    }

    public async Task<QuizDto?> UpdateQuizAsync(int id, UpdateQuizDto quiz)
    {
        var existingQuiz = await context.Quizzes.FindAsync(id);
        if (existingQuiz == null)
        {
            return null;
        }
        existingQuiz.Title = quiz.Title;
        existingQuiz.Description = quiz.Description;
        await context.SaveChangesAsync();
        return new QuizDto
        {
            Id = existingQuiz.Id,
            Title = existingQuiz.Title,
            Description = existingQuiz.Description,
        };
    }

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
}