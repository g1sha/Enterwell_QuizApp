using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class QuizContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Question> Questions { get; set; }
    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<QuizQuestion> QuizQuestions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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
