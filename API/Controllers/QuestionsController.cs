using Core.DTOs.Question;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionsController : ControllerBase
{
    private readonly IQuestionService _questionService;

    public QuestionsController(IQuestionService questionService)
    {
        _questionService = questionService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<QuestionDto>>> GetQuestions()
    {
        var questions = await _questionService.GetAllQuestionsAsync();
        return Ok(questions);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<QuestionDto>> GetQuestion(int id)
    {
        var question = await _questionService.GetQuestionByIdAsync(id);
        
        if (question == null)
        {
            return NotFound();
        }

        return Ok(question);
    }

    [HttpPost]
    public async Task<ActionResult<QuestionDto>> CreateQuestion([FromBody] CreateQuestionDto dto)
    {
        var createdQuestion = await _questionService.CreateQuestionAsync(dto);
        return CreatedAtAction(nameof(GetQuestion), new { id = createdQuestion.Id }, createdQuestion);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateQuestion(int id, [FromBody] UpdateQuestionDto dto)
    {
        await _questionService.UpdateQuestionAsync(id, dto);
        return NoContent();
    }
}