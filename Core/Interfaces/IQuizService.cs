using Core.DTOs.Pagination;
using Core.DTOs.Quiz;
using Core.Entities;

namespace Core.Interfaces;

public interface IQuizService
{
    Task<PaginatedResponseDto<QuizDto>> GetAllQuizzesAsync(int pageNumber, int pageSize);
    Task<QuizDetailDto?> GetQuizByIdAsync(int id);
    Task<QuizDto> CreateQuizAsync(CreateQuizDto quiz);
    Task<QuizDto?> UpdateQuizAsync(int id, UpdateQuizDto quiz);
    Task<bool> DeleteQuizAsync(int id);
    Task<(bool Success, string? Error)> AddQuestionToQuizAsync(int quizId, AddQuestionToQuizDto dto);
    Task<(bool Success, string? Error)> RemoveQuestionFromQuizAsync(int quizId, AddQuestionToQuizDto dto);
}


