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
    public DbSet<UserInfo> UserInfos { get; set; }
    public DbSet<UserSystemInfo> UserSystemInfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite(_connectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserInfo>()
            .HasOne(u => u.BotUser)
            .WithMany()
            .HasForeignKey(u => u.UserId);        
        modelBuilder.Entity<UserSystemInfo>()
            .HasOne(u => u.BotUser)
            .WithMany()
            .HasForeignKey(u => u.UserId);
    }

    private BotDbContext() : base("Filename=coffee.db")
    {
        Database.EnsureCreated();

        var creator = this.GetService<IRelationalDatabaseCreator>();
        if (!creator.Exists())
            creator.CreateTables();
    }
}