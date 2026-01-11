using System.Text;
using API.Extensions;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();

// Swagger from Extensions
builder.Services.AddSwaggerDocumentation();

// Identity and JWT from Extensions
builder.Services.AddIdentityServices(builder.Configuration);

builder.Services.AddDbContext<QuizContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Export service factory
var pluginPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins", "Export");
Directory.CreateDirectory(pluginPath);
builder.Services.AddSingleton<IExportServiceFactory>(new ExportServiceFactory(pluginPath));

// Application services
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IQuizService, QuizService>();

var app = builder.Build();

// Middleware

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Seed Data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<QuizContext>();
    await InitialSeed.SeedAsync(context, scope.ServiceProvider.GetRequiredService<UserManager<User>>());
}

app.Run();
