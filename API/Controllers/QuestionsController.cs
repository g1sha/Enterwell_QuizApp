using Core.Constants;
using Core.DTOs;
using Core.DTOs.Pagination;
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
    public async Task<ActionResult<PaginatedResponseDto<QuestionDto>>> GetQuestions([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? filter = null)
    {
        var questions = await questionService.GetAllQuestionsAsync(pageNumber, pageSize, filter);
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