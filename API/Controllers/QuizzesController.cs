using Core.Constants;
using Core.DTOs;
using Core.DTOs.Pagination;
using Core.DTOs.Question;
using Core.DTOs.Quiz;
using Core.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizzesController(IQuizService quizService) : ControllerBase
{
    /// <summary>
    ///  Retrieves a paginated list of quizzes with optional page number and page size parameters.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<PaginatedResponseDto<QuizDto>>> GetQuizzes([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var quizzes = await quizService.GetAllQuizzesAsync(pageNumber, pageSize);
        return Ok(quizzes);
    }

    /// <summary>
    /// Retrieves a specific quiz by its ID. 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<QuizDto>> GetQuiz(int id)
    {
        var quiz = await quizService.GetQuizByIdAsync(id);
        if (quiz == null)
            return NotFound(ErrorResponse.Create(ErrorMessages.QuizNotFound));
        return Ok(quiz);
    }

    /// <summary>
    /// Creates a new quiz with the provided details.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<QuizDto>> CreateQuiz([FromBody] CreateQuizDto dto)
    {
        var createdQuiz = await quizService.CreateQuizAsync(dto);
        return CreatedAtAction(nameof(GetQuiz), new { id = createdQuiz.Id }, createdQuiz);
    }

    /// <summary>
    /// Updates an existing quiz identified by its ID with the provided details.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateQuiz(int id, [FromBody] UpdateQuizDto dto)
    {
        await quizService.UpdateQuizAsync(id, dto);
        return NoContent();
    }

    /// <summary>
    /// Deletes a quiz identified by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteQuiz(int id)
    {
        var deleted = await quizService.DeleteQuizAsync(id);
        if (!deleted)
            return NotFound(ErrorResponse.Create(ErrorMessages.QuizNotFound));
        return NoContent();
    }
    
    /// <summary>
    /// Adds a question to a quiz identified by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("{id:int}/questions")]
    public async Task<ActionResult> AddQuestionToQuiz(int id, [FromBody] AddQuestionToQuizDto dto)
    {
        var (success, error) = await quizService.AddQuestionToQuizAsync(id, dto);
        if (!success)
            return BadRequest(ErrorResponse.Create(error!));
        return NoContent();
    }
    
    /// <summary>
    /// Removes a question from a quiz identified by its ID, without removing the question from the database.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="questionId"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}/questions")]
    public async Task<ActionResult> RemoveQuestionFromQuiz(int id, int questionId)
    {
        var (success, error) = await quizService.RemoveQuestionFromQuizAsync(id, new AddQuestionToQuizDto { QuestionId = questionId });
        if (!success)
            return BadRequest(ErrorResponse.Create(error!));
        return NoContent();
    }

    /// <summary>
    /// Exports the questions of a quiz in the specified format. (CSV, JSON, XML...)
    /// </summary>
    /// <param name="id"></param>
    /// <param name="format"></param>
    /// <param name="factory"></param>
    /// <returns></returns>
    [HttpGet("{id}/export")]
    public async Task<IActionResult> ExportQuiz(
        int id,
        [FromQuery] string format,
        [FromServices] IExportServiceFactory factory)
    {
        var quiz = await quizService.GetQuizByIdAsync(id);
        if (quiz == null)
            return NotFound();

        var loader = factory.GetLoader();
    
        try
        {
            // removed answers from export after reading instructions again
            var fileBytes = loader.Export(format, quiz.Questions
                .Select(q => new ExportQuestionDto
                {
                    Id = q.Id,
                    Text = q.Text,
                }));
            var formatInfo = loader.GetAvailableFormats()
                .First(f => f.Format.Equals(format, StringComparison.OrdinalIgnoreCase));
        
            return File(fileBytes, formatInfo.ContentType, $"quiz-{id}{formatInfo.FileExtension}");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

