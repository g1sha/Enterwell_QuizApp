using Core.DTOs.Question;
using Core.Entities;

namespace Core.Interfaces;

public interface IQuestionService
{
    Task<IEnumerable<QuestionDto>> GetAllQuestionsAsync();
    Task<QuestionDto?> GetQuestionByIdAsync(int id);
    Task<QuestionDto> CreateQuestionAsync(CreateQuestionDto question);
    Task<QuestionDto?> UpdateQuestionAsync(int id, UpdateQuestionDto question);
}

