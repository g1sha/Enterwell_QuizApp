using System.ComponentModel.DataAnnotations;
using Core.Constants;
using Core.DTOs;
using Core.DTOs.Pagination;
using Core.DTOs.Question;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class QuestionsController(IQuestionService questionService) : ControllerBase
{
    /// <summary>
    /// Get paginated list of questions with optional filtering on question text
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<PaginatedResponseDto<QuestionDto>>> GetQuestions([FromQuery][Range(1,int.MaxValue)] int pageNumber = 1, [FromQuery][Range(1,100)] int pageSize = 10, [FromQuery] string? filter = null)
    {
        var questions = await questionService.GetAllQuestionsAsync(pageNumber, pageSize, filter);
        return Ok(questions);
    }
    
    /// <summary>
    /// Get a specific question by its ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<QuestionDto>> GetQuestion(int id)
    {
        var question = await questionService.GetQuestionByIdAsync(id);
        
        if (question == null)
            return NotFound(ErrorResponse.Create(ErrorMessages.QuestionNotFound));
        return Ok(question);
    }

    /// <summary>
    /// Create a new question
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<QuestionDto>> CreateQuestion([FromBody] CreateQuestionDto dto)
    {
        var createdQuestion = await questionService.CreateQuestionAsync(dto);
        return CreatedAtAction(nameof(GetQuestion), new { id = createdQuestion.Id }, createdQuestion);
    }

    /// <summary>
    /// Update an existing question by its ID
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateQuestion(int id, [FromBody] UpdateQuestionDto dto)
    {
        var updatedQuestion=await questionService.UpdateQuestionAsync(id, dto);
        if (updatedQuestion == null)
            return NotFound(ErrorResponse.Create(ErrorMessages.QuestionNotFound));
        return NoContent();
    }
}