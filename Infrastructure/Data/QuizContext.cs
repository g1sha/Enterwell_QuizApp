using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class QuizContext(DbContextOptions options) : IdentityDbContext<User>(options)
{
    public DbSet<Question> Questions { get; set; }
    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<QuizQuestion> QuizQuestions { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure unique constraint on QuizQuestion for (QuizId, QuestionId)
        // making sure the same question is not added multiple times to the same quiz
        // and set delete behavior to NoAction to prevent cascading deletes
        modelBuilder.Entity<QuizQuestion>(entity =>
        {
            entity.HasIndex(qq => new { qq.QuizId, qq.QuestionId })
                  .IsUnique();

            entity.HasOne(qq => qq.Question)
                  .WithMany(q => q.QuizQuestions)
                  .HasForeignKey(qq => qq.QuestionId)
                  .OnDelete(DeleteBehavior.NoAction);
        });
    }
}
