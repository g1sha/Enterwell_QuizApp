using Core.DTOs.Quiz;
using Core.Entities;

namespace Core.Interfaces;

public interface IQuizService
{
    Task<IEnumerable<QuizDto>> GetAllQuizzesAsync();
    Task<QuizDto?> GetQuizByIdAsync(int id);
    Task<QuizDto> CreateQuizAsync(CreateQuizDto quiz);
    Task<QuizDto?> UpdateQuizAsync(int id, UpdateQuizDto quiz);
    Task DeleteQuizAsync(int id);
}


