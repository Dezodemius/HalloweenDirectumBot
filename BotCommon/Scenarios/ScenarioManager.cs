using System.Collections.Generic;
using BotCommon;
using Microsoft.EntityFrameworkCore;

namespace Directum238Bot;

public class ScenarioManager : DbContext
{
  public DbSet<UserScenario> UserScenarios { get; set; }

  public DbSet<ChatScenario> ChatScenarios { get; set; }

  private string connectionString;
  
  public UserScenario Get

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseSqlite(this.connectionString);
  }

  public ScenarioManager(string connectionString)
  {
    this.connectionString = connectionString;
    Database.EnsureCreated();
  }
}