using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BotCommon.Scenarios;

public sealed class UserScenarioRepository : DbContext
{
  private DbSet<UserCommandScenario> UserScenarios { get; set; }

  private readonly string connectionString;

  public bool TryGet(long userId, out UserCommandScenario userCommandScenario)
  {
    userCommandScenario = this.Get(userId);
    return userCommandScenario == null;
  }

  public UserCommandScenario Get(long userId)
  {
    var foundedScenario = this.UserScenarios.SingleOrDefault(s => s.UserId == userId);
    if (foundedScenario == null)
      return null;

    var chatScenario = BotCommandScenarioCache.ChatScenarios.SingleOrDefault(s => s.Id == foundedScenario.ChatScenarioGuid);
    foundedScenario.CommandScenario = chatScenario;
    return foundedScenario;
  }

  public void Remove(long userId)
  {
    var foundedScenario = this.Get(userId);
    foundedScenario.CommandScenario.Reset();
    this.UserScenarios.Remove(foundedScenario);
    this.SaveChanges();
  }

  public void Remove(UserCommandScenario commandScenario)
  {
    this.Remove(commandScenario.UserId);
  }

  public void Add(UserCommandScenario userCommandScenario)
  {
    if (userCommandScenario == null)
      return;
    this.UserScenarios.Add(userCommandScenario);
    this.SaveChanges();
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseSqlite(this.connectionString);
  }

  public UserScenarioRepository(string connectionString)
  {
    this.connectionString = connectionString;
    Database.EnsureCreated();
  }
}