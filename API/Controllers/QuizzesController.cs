using Core.Constants;
using Core.DTOs;
using Core.DTOs.Pagination;
using Core.DTOs.Quiz;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizzesController(IQuizService quizService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedResponseDto<QuizDto>>> GetQuizzes([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var quizzes = await quizService.GetAllQuizzesAsync(pageNumber, pageSize);
        return Ok(quizzes);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<QuizDto>> GetQuiz(int id)
    {
        var quiz = await quizService.GetQuizByIdAsync(id);
        if (quiz == null)
            return NotFound(ErrorResponse.Create(ErrorMessages.QuizNotFound));
        return Ok(quiz);
    }

    [HttpPost]
    public async Task<ActionResult<QuizDto>> CreateQuiz([FromBody] CreateQuizDto dto)
    {
        var createdQuiz = await quizService.CreateQuizAsync(dto);
        return CreatedAtAction(nameof(GetQuiz), new { id = createdQuiz.Id }, createdQuiz);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateQuiz(int id, [FromBody] UpdateQuizDto dto)
    {
        await quizService.UpdateQuizAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteQuiz(int id)
    {
        var deleted = await quizService.DeleteQuizAsync(id);
        if (!deleted)
            return NotFound(ErrorResponse.Create(ErrorMessages.QuizNotFound));
        return NoContent();
    }
    
    [HttpPost("{id:int}/questions")]
    public async Task<ActionResult> AddQuestionToQuiz(int id, [FromBody] AddQuestionToQuizDto dto)
    {
        var (success, error) = await quizService.AddQuestionToQuizAsync(id, dto);
        if (!success)
            return BadRequest(ErrorResponse.Create(error!));
        return NoContent();
    }
}

