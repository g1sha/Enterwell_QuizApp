using Core.Constants;
using Core.DTOs;
using Core.DTOs.Question;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionsController(IQuestionService questionService) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<QuestionDto>>> GetQuestions()
    {
        var questions = await questionService.GetAllQuestionsAsync();
        return Ok(questions);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<QuestionDto>> GetQuestion(int id)
    {
        var question = await questionService.GetQuestionByIdAsync(id);
        
        if (question == null)
            return NotFound(ErrorResponse.Create(ErrorMessages.QuestionNotFound));
        return Ok(question);
    }

    [HttpPost]
    public async Task<ActionResult<QuestionDto>> CreateQuestion([FromBody] CreateQuestionDto dto)
    {
        var createdQuestion = await questionService.CreateQuestionAsync(dto);
        return CreatedAtAction(nameof(GetQuestion), new { id = createdQuestion.Id }, createdQuestion);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateQuestion(int id, [FromBody] UpdateQuestionDto dto)
    {
        await questionService.UpdateQuestionAsync(id, dto);
        return NoContent();
    }
}