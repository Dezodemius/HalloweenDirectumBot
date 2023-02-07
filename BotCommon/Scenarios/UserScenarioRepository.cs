using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BotCommon.Scenarios;

public sealed class UserScenarioRepository : DbContext
{
  private DbSet<UserScenario> UserScenarios { get; set; }

  private readonly string connectionString;

  public UserScenario Get(long userId)
  {
    var foundedScenario = this.UserScenarios.SingleOrDefault(s => s.UserId == userId);
    if (foundedScenario == null)
      return null;

    var chatScenario = ChatScenarioCache.ChatScenarios.SingleOrDefault(s => s.Id == foundedScenario.ChatScenarioGuid);
    foundedScenario.ChatScenario = chatScenario;
    return foundedScenario;
  }

  public void Remove(long userId)
  {
    var foundedScenario = this.Get(userId);
    this.UserScenarios.Remove(foundedScenario);
    this.SaveChanges();
  }

  public void Remove(UserScenario scenario)
  {
    this.Remove(scenario.UserId);
  }

  public void Add(UserScenario userScenario)
  {
    this.UserScenarios.Add(userScenario);
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