using BotCommon.Repository;
using DirectumCareerNightBot.Quiz;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace DirectumCareerNightBot;

public sealed class BotDbContext : UserDbContext
{
    public DbSet<QuizQuestion> Questions { get; set; }
    public DbSet<QuizPossibleAnswer> Choices { get; set; }
    public DbSet<QuizUserQuestion> UserQuestions { get; set; }
    public DbSet<QuizUserResult> UserResults { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite(_connectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<QuizQuestion>()
            .HasOne(q => q.CorrectChoice)
            .WithMany()
            .HasForeignKey(q => q.CorrectChoiceId);

        modelBuilder.Entity<QuizPossibleAnswer>()
            .HasOne(pa => pa.Question)
            .WithMany(q => q.Choices)
            .HasForeignKey(pa => pa.QuestionId);

        modelBuilder.Entity<QuizUserQuestion>()
            .HasOne(quq => quq.User)
            .WithMany()
            .HasForeignKey(quq => quq.UserId);
        modelBuilder.Entity<QuizUserResult>()
            .HasOne(r => r.BotUser)
            .WithMany()
            .HasForeignKey(r => r.UserId);
    }

    public BotDbContext() : base("Filename=quiz.db")
    {
        Database.EnsureCreated();

        var creator = this.GetService<IRelationalDatabaseCreator>();
        if (!creator.Exists())
            creator.CreateTables();
    }
}