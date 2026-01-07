using Core.DTOs.Quiz;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizzesController : ControllerBase
{
    private readonly IQuizService _quizService;

    public QuizzesController(IQuizService quizService)
    {
        _quizService = quizService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<QuizDto>>> GetQuizzes()
    {
        var quizzes = await _quizService.GetAllQuizzesAsync();
        return Ok(quizzes);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<QuizDto>> GetQuiz(int id)
    {
        var quiz = await _quizService.GetQuizByIdAsync(id);
        
        if (quiz == null)
        {
            return NotFound();
        }

        return Ok(quiz);
    }

    [HttpPost]
    public async Task<ActionResult<QuizDto>> CreateQuiz([FromBody] CreateQuizDto dto)
    {
        var createdQuiz = await _quizService.CreateQuizAsync(dto);
        return CreatedAtAction(nameof(GetQuiz), new { id = createdQuiz.Id }, createdQuiz);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateQuiz(int id, [FromBody] UpdateQuizDto dto)
    {
        await _quizService.UpdateQuizAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteQuiz(int id)
    {
        await _quizService.DeleteQuizAsync(id);
        return NoContent();
    }
}

