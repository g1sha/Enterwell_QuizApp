using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public static class InitialSeed
{
    public static async Task SeedAsync(QuizContext context, UserManager<User> userManager)
    {
        // Initial migration
        await context.Database.MigrateAsync();
        
        // for assigning created by
        var random = new Random();
        var generatedUsers=new List<User>();
        //populate db with data
        if (!await context.Users.AnyAsync())
        {
            var users = new List<User>
            {
                new() { Email = "rejd@enterwell.net", UserName = "rejd@enterwell.net", FirstName = "Rejd", LastName = "Quizmaster", RefreshToken = Guid.NewGuid().ToString(), RefreshTokenExpiryTime =  DateTime.UtcNow.AddDays(7) },
                new() { Email = "igor@enterwell.net", UserName = "igor@enterwell.net", FirstName = "Igor", LastName = "Bošnjak", RefreshToken = Guid.NewGuid().ToString(), RefreshTokenExpiryTime =  DateTime.UtcNow.AddDays(7)  },
            };

            foreach (var user in users)
                await userManager.CreateAsync(user, "Password123!");
            
            generatedUsers= users;
        }
        
        if (!await context.Questions.AnyAsync())
        {
            var questions = new List<Question>
            {
                new() { Text = "Šta je varijabla?", CorrectAnswer = "Mjesto za čuvanje vrijednosti u memoriji" },
                new() { Text = "Kojim simbolom se najčešće završava naredba u C# ?", CorrectAnswer = ";" },
                new() { Text = "Koja naredba se koristi za ispisivanje teksta u konzoli u C#?", CorrectAnswer = "Console.WriteLine" },
                new() { Text = "Šta je 'if' u programiranju?", CorrectAnswer = "Uslov" },
                new() { Text = "Kojim simbolima se označava jednolinijski komentar u C# ?", CorrectAnswer = "//" },

                new() { Text = "Koji je glavni grad Hrvatske?", CorrectAnswer = "Zagreb" },
                new() { Text = "Koji je glavni grad Srbije?", CorrectAnswer = "Beograd" },
                new() { Text = "Koji je najveći kontinent na Zemlji?", CorrectAnswer = "Azija" },
                new() { Text = "Koja rijeka prolazi kroz Mostar?", CorrectAnswer = "Neretva" },
                new() { Text = "Na kojem kontinentu se nalazi Slovenija?", CorrectAnswer = "Europa" },

                new() { Text = "Koliko je 10 + 5?", CorrectAnswer = "15" },
                new() { Text = "Koliko dana ima jedna sedmica?", CorrectAnswer = "7" },
                new() { Text = "Koje godišnje doba dolazi poslije proljeća?", CorrectAnswer = "Ljeto" },
                new() { Text = "Koja je najveća životinja na svijetu? ", CorrectAnswer = "Plavi kit" },
                new() { Text = "Koja boja nastaje miješanjem plave i žute?", CorrectAnswer = "Zelena" },
            };
            foreach (var question in questions)
            {
                var randomUser = generatedUsers[random.Next(generatedUsers.Count)];
                question.SetCreatedBy(randomUser.Id);
            }

            await context.Questions.AddRangeAsync(questions);
            await context.SaveChangesAsync();
        }

        if (!await context.Quizzes.AnyAsync())
        {
            var questions = await context.Questions.ToListAsync();

            var quizzes = new List<Quiz>
            {
                new() { Title = "Programiranje", Description = "Osnovna pitanja iz programiranja" },
                new() { Title = "Geografija", Description = "Osnovna pitanja iz geografije" },
                new() { Title = "Opšte znanje", Description = String.Empty },
                new() { Title = "Mix znanja", Description = "Kombinacija svih pitanja" },
            };
            
            foreach (var quiz in quizzes)
            {
                var randomUser = generatedUsers[random.Next(generatedUsers.Count)];
                quiz.SetCreatedBy(randomUser.Id);
            }
            
            await context.Quizzes.AddRangeAsync(quizzes);
            await context.SaveChangesAsync();

            var quiz1 = await context.Quizzes.FirstAsync();
            var quiz2 = await context.Quizzes.Skip(1).FirstAsync();

            if (!await context.QuizQuestions.AnyAsync())
            {
                var quizQuestions = new List<QuizQuestion>
                {
                    new() { QuizId = quizzes[0].Id, QuestionId = questions[0].Id, Quiz = quizzes[0], Question = questions[0] },
                    new() { QuizId = quizzes[0].Id, QuestionId = questions[1].Id, Quiz = quizzes[0], Question = questions[1] },
                    new() { QuizId = quizzes[0].Id, QuestionId = questions[2].Id, Quiz = quizzes[0], Question = questions[2] },
                    new() { QuizId = quizzes[0].Id, QuestionId = questions[3].Id, Quiz = quizzes[0], Question = questions[3] },
                    new() { QuizId = quizzes[0].Id, QuestionId = questions[4].Id, Quiz = quizzes[0], Question = questions[4] },

                    new() { QuizId = quizzes[1].Id, QuestionId = questions[5].Id, Quiz = quizzes[1], Question = questions[5] },
                    new() { QuizId = quizzes[1].Id, QuestionId = questions[6].Id, Quiz = quizzes[1], Question = questions[6] },
                    new() { QuizId = quizzes[1].Id, QuestionId = questions[7].Id, Quiz = quizzes[1], Question = questions[7] },
                    new() { QuizId = quizzes[1].Id, QuestionId = questions[8].Id, Quiz = quizzes[1], Question = questions[8] },
                    new() { QuizId = quizzes[1].Id, QuestionId = questions[9].Id, Quiz = quizzes[1], Question = questions[9] },

                    new() { QuizId = quizzes[2].Id, QuestionId = questions[10].Id, Quiz = quizzes[2], Question = questions[10] },
                    new() { QuizId = quizzes[2].Id, QuestionId = questions[11].Id, Quiz = quizzes[2], Question = questions[11] },
                    new() { QuizId = quizzes[2].Id, QuestionId = questions[12].Id, Quiz = quizzes[2], Question = questions[12] },
                    new() { QuizId = quizzes[2].Id, QuestionId = questions[13].Id, Quiz = quizzes[2], Question = questions[13] },
                    new() { QuizId = quizzes[2].Id, QuestionId = questions[14].Id, Quiz = quizzes[2], Question = questions[14] },

                    new() { QuizId = quizzes[3].Id, QuestionId = questions[0].Id,  Quiz = quizzes[3], Question = questions[0] },
                    new() { QuizId = quizzes[3].Id, QuestionId = questions[1].Id,  Quiz = quizzes[3], Question = questions[1] },
                    new() { QuizId = quizzes[3].Id, QuestionId = questions[2].Id,  Quiz = quizzes[3], Question = questions[2] },
                    new() { QuizId = quizzes[3].Id, QuestionId = questions[3].Id,  Quiz = quizzes[3], Question = questions[3] },
                    new() { QuizId = quizzes[3].Id, QuestionId = questions[4].Id,  Quiz = quizzes[3], Question = questions[4] },
                    new() { QuizId = quizzes[3].Id, QuestionId = questions[5].Id,  Quiz = quizzes[3], Question = questions[5] },
                    new() { QuizId = quizzes[3].Id, QuestionId = questions[6].Id,  Quiz = quizzes[3], Question = questions[6] },
                    new() { QuizId = quizzes[3].Id, QuestionId = questions[7].Id,  Quiz = quizzes[3], Question = questions[7] },
                    new() { QuizId = quizzes[3].Id, QuestionId = questions[8].Id,  Quiz = quizzes[3], Question = questions[8] },
                    new() { QuizId = quizzes[3].Id, QuestionId = questions[9].Id,  Quiz = quizzes[3], Question = questions[9] },
                    new() { QuizId = quizzes[3].Id, QuestionId = questions[10].Id, Quiz = quizzes[3], Question = questions[10] },
                    new() { QuizId = quizzes[3].Id, QuestionId = questions[11].Id, Quiz = quizzes[3], Question = questions[11] },
                    new() { QuizId = quizzes[3].Id, QuestionId = questions[12].Id, Quiz = quizzes[3], Question = questions[12] },
                    new() { QuizId = quizzes[3].Id, QuestionId = questions[13].Id, Quiz = quizzes[3], Question = questions[13] },
                    new() { QuizId = quizzes[3].Id, QuestionId = questions[14].Id, Quiz = quizzes[3], Question = questions[14] },
                };
                foreach (var qq in quizQuestions)
                {
                    var randomUser = generatedUsers[random.Next(generatedUsers.Count)];
                    qq.SetCreatedBy(randomUser.Id);
                }

                await context.QuizQuestions.AddRangeAsync(quizQuestions);
                await context.SaveChangesAsync();
            }
        }
    }
}