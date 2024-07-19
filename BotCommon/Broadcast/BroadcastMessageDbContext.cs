using System;
using System.Collections.Generic;
using BotCommon.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace BotCommon.Broadcast;

public class BroadcastMessageDbContext : DbContext
{
  private static readonly object padlock = new object();
  private static volatile BroadcastMessageDbContext instance;
  private static Lazy<BroadcastMessageDbContext> lazy = new(() => new BroadcastMessageDbContext());

  public static BroadcastMessageDbContext Instance
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

  public DbSet<BroadcastMessageUser> BroadcastMessageUsers { get; set; }

  public void InitBroadcastUsers(IEnumerable<BotUser> users)
  {
    foreach (var botUser in users)
    {
      this.BroadcastMessageUsers.Add(new BroadcastMessageUser(botUser.Id, DateTime.MinValue, false, false));
    }

    this.SaveChanges();
  }
  
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseSqlite("Filename=broadcast.db");
  }

  public BroadcastMessageDbContext()
  {
    Database.EnsureCreated();

    var creator = this.GetService<IRelationalDatabaseCreator>();
    if (!creator.Exists())
      creator.CreateTables();
  }
}