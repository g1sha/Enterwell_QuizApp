using Core.DTOs.Quiz;
using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace QuizApp.Tests;

public class UtQuiz
{
    // Creates a fresh in memory database for each test
    private static QuizContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<QuizContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
            .Options;

        return new QuizContext(options);
    }

    // Creates the service under test with mocked dependencies
    private static QuizService CreateQuizService(QuizContext context)
    {
        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        return new QuizService(context, mockHttpContextAccessor.Object);
    }

    // Create a test quiz entity
    private static Quiz CreateTestQuiz(string title = "Test Quiz", string description = "Test Description")
    {
        return new Quiz { Title = title, Description = description };
    }

    // Create a test quiz DTO
    private static CreateQuizDto CreateTestQuizDto(string title = "Test Quiz", string description = "Test Description")
    {
        return new CreateQuizDto { Title = title, Description = description };
    }

    // Tests: Getting a quiz by valid ID returns the correct quiz
    [Fact]
    public async Task GetQuizById_WithValidId_ReturnsQuiz()
    {
        await using var context = CreateInMemoryContext();
        context.Quizzes.Add(CreateTestQuiz());
        await context.SaveChangesAsync();
        var service = CreateQuizService(context);

        var result = await service.GetQuizByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Test Quiz", result.Title);
    }

    // Tests: Getting a quiz by non-existent ID returns null
    [Fact]
    public async Task GetQuizById_WithInvalidId_ReturnsNull()
    {
        await using var context = CreateInMemoryContext();
        var service = CreateQuizService(context);

        var result = await service.GetQuizByIdAsync(999);

        Assert.Null(result);
    }

    // Tests: Getting all quizzes returns the correct count
    [Fact]
    public async Task GetAllQuizzes_ReturnsAllQuizzes()
    {
        await using var context = CreateInMemoryContext();
        context.Quizzes.AddRange(
            CreateTestQuiz("Quiz 1"),
            CreateTestQuiz("Quiz 2"),
            CreateTestQuiz("Quiz 3"));
        await context.SaveChangesAsync();
        var service = CreateQuizService(context);

        var result = await service.GetAllQuizzesAsync();

        Assert.Equal(3, result.Items.Count());
    }

    // Tests: Getting all quizzes from empty database returns empty list
    [Fact]
    public async Task GetAllQuizzes_WhenEmpty_ReturnsEmptyList()
    {
        await using var context = CreateInMemoryContext();
        var service = CreateQuizService(context);

        var result = await service.GetAllQuizzesAsync();

        Assert.Empty(result.Items);
    }

    // Tests: Creating a quiz adds it to the database
    [Fact]
    public async Task CreateQuiz_AddsQuizToDatabase()
    {
        await using var context = CreateInMemoryContext();
        var service = CreateQuizService(context);
        var quizDto = CreateTestQuizDto("New Quiz");

        var result = await service.CreateQuizAsync(quizDto);

        Assert.NotNull(result);
        Assert.Equal("New Quiz", result.Title);
        Assert.Equal(1, await context.Quizzes.CountAsync());
    }

    // Tests: Updating a quiz modifies the existing record
    [Fact]
    public async Task UpdateQuiz_WithValidQuiz_UpdatesDatabase()
    {
        await using var context = CreateInMemoryContext();
        context.Quizzes.Add(CreateTestQuiz());
        await context.SaveChangesAsync();
        var service = CreateQuizService(context);

        var updateDto = new UpdateQuizDto { Title = "Updated Title", Description = "Updated Description" };
        await service.UpdateQuizAsync(1, updateDto);

        var updated = await context.Quizzes.FindAsync(1);
        Assert.Equal("Updated Title", updated?.Title);
    }

    // Tests: Deleting a quiz removes it from the database
    [Fact]
    public async Task DeleteQuiz_WithValidId_RemovesFromDatabase()
    {
        await using var context = CreateInMemoryContext();
        context.Quizzes.Add(CreateTestQuiz());
        await context.SaveChangesAsync();
        var service = CreateQuizService(context);

        await service.DeleteQuizAsync(1);

        Assert.Equal(0, await context.Quizzes.CountAsync());
    }

    // Tests: Deleting a not existing quiz does not throw an exception
    [Fact]
    public async Task DeleteQuiz_WithInvalidId_DoesNotThrow()
    {
        await using var context = CreateInMemoryContext();
        var service = CreateQuizService(context);

        var exception = await Record.ExceptionAsync(() => service.DeleteQuizAsync(999));

        Assert.Null(exception);
    }
}
