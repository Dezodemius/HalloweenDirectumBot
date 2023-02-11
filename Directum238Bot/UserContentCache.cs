using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types.Enums;

namespace Directum238Bot;

public sealed class UserContentCache : DbContext
{
  private readonly string _connectionString;

  public DbSet<UserContent> UserContents { get; set; }

  public UserContent GetRandomContentExceptCurrent(long userId, MessageType type)
  {
    var founded = UserContents.Where(c => c.UserId != userId && c.Type == type);
    var randomIndex = new Random().Next(founded.Count());
    return founded.ToList().ElementAtOrDefault(randomIndex);
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseSqlite(this._connectionString);
  }

  public UserContentCache(string connectionString)
  {
    this._connectionString = connectionString;
    Database.EnsureCreated();
  }
}

[PrimaryKey(nameof(Id))]
public class UserContent
{
  public Guid Id { get; set; }
  public long UserId { get; set; }
  public string Content { get; set; }
  public MessageType Type { get; set; }

  public UserContent(long userId, string content, MessageType type)
  {
    this.Id  = new Guid();
    this.UserId = userId;
    this.Content = content;
    this.Type = type;
  }
}