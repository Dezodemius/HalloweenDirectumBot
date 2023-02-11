using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BotCommon;

public class ActiveUsersManager : DbContext
{
  private readonly string _connectionString;

  public DbSet<BotUser> BotUsers { get; set; }

  public BotUser GetUser(long id)
  {
    var user = this.BotUsers.SingleOrDefault(u => u.BotUserId == id);
    return user;
  }

  public void AddUser(BotUser user)
  {
    if (this.GetUser(user.BotUserId) != null)
      return;
    this.BotUsers.Add(user);
    this.SaveChanges();
  }

  public void DeleteUser(BotUser user)
  {
    this.BotUsers.Remove(user);
    this.SaveChanges();
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseSqlite(this._connectionString);
  }

  public ActiveUsersManager(string connectionString)
  {
    this._connectionString = connectionString;
    Database.EnsureCreated();
  }
}

[PrimaryKey(nameof(Id))]
public class BotUser
{
  public Guid Id { get; set; }
  public long BotUserId { get; set; }

  public BotUser()
  {
  
  }

  public BotUser(long id)
  {
    this.Id = new Guid();
    this.BotUserId = id;
  }
}