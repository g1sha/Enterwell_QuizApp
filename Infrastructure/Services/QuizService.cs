using Core.DTOs.Pagination;
using Core.DTOs.Question;
using Core.DTOs.Quiz;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class QuizService(QuizContext context) : IQuizService
{
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

    public async Task<bool> AddQuestionToQuizAsync(int quizId, AddQuestionToQuizDto dto)
    {
        var already_exists = await context.QuizQuestions.AnyAsync(q => q.QuestionId == dto.QuestionId && q.QuizId == quizId);
        
        if (already_exists)
            return false;

        var quiz = await context.Quizzes.FindAsync(quizId);
        var question = await context.Questions.FindAsync(dto.QuestionId);
        
        if (quiz == null || question == null)
            return false;
                
        var quizQuestion = new QuizQuestion
        {
            Quiz = quiz,
            Question = question,
        };
        
        context.QuizQuestions.Add(quizQuestion);
        await context.SaveChangesAsync();
        return true;
    }
}