using BotCommon.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace DirectumCoffee;

public sealed class BotDbContext : UserDbContext
{
    private static readonly object padlock = new object();
    private static volatile BotDbContext instance;
    private static Lazy<BotDbContext> lazy = new(() => new BotDbContext());

    public static BotDbContext Instance
    {
        get
        {
            if (instance == null)
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = lazy.Value;
                    }
                }
            }
            return instance;
        }
    } 
    public DbSet<UserInfo> Questions { get; set; }
    public DbSet<UserSystemInfo> Choices { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite(_connectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<QuizQuestion>()
        //     .HasOne(q => q.CorrectChoice)
        //     .WithMany()
        //     .HasForeignKey(q => q.CorrectChoiceId);
        //
        // modelBuilder.Entity<QuizPossibleAnswer>()
        //     .HasOne(pa => pa.Question)
        //     .WithMany(q => q.Choices)
        //     .HasForeignKey(pa => pa.QuestionId);
        // modelBuilder.Entity<QuizUserQuestion>()
        //     .HasOne(quq => quq.User)
        //     .WithMany()
        //     .HasForeignKey(quq => quq.UserId);
        // modelBuilder.Entity<QuizUserResult>()
        //     .HasOne(r => r.BotUser)
        //     .WithMany()
        //     .HasForeignKey(r => r.UserId);
        // modelBuilder.Entity<UserData>()
        //     .HasOne(u => u.BotUser)
        //     .WithMany()
        //     .HasForeignKey(u => u.UserId);
    }

    private BotDbContext() : base("Filename=quiz.db")
    {
        Database.EnsureCreated();

        var creator = this.GetService<IRelationalDatabaseCreator>();
        if (!creator.Exists())
            creator.CreateTables();
    }
}