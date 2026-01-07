using Core.DTOs.Quiz;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Services;

public class QuizService : IQuizService
{
    public async Task<IEnumerable<QuizDto>> GetAllQuizzesAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<QuizDto?> GetQuizByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<QuizDto> CreateQuizAsync(CreateQuizDto quiz)
    {
        throw new NotImplementedException();
    }

    public async Task<QuizDto?> UpdateQuizAsync(int id, UpdateQuizDto quiz)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteQuizAsync(int id)
    {
        throw new NotImplementedException();
    }
}