using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace BotCommon.Repository;

public class UserDbContext : DefaultDbContext<BotUser>
{
  public DbSet<BotUser> BotUsers { get; set; }

  public override BotUser Get(BotUser user)
  {
    return this.BotUsers.SingleOrDefault(u => u.Id == user.Id);
  }

  public override IEnumerable<BotUser> GetAll()
  {
    return this.BotUsers;
  }

  public override void Add(BotUser user)
  {
    if (this.Get(user) != null)
      return;
    this.BotUsers.Add(user);
    base.Add(user);
  }

  public override void Delete(BotUser user)
  {
    this.BotUsers.Remove(user);
    base.Delete(user);
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseSqlite(this._connectionString);
  }

  public UserDbContext() : this("Filename=app.db")
  {
    
  }
  
  public UserDbContext(string connectionString) : base(connectionString)
  {
  }
}

public class BotUser
{
  [Key]
  public long Id { get; set; }
  public string Username { get; set; }
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public string UserLanguage { get; set; }
  public DateTime FirstMeet { get; set; }

  public BotUser()
  {
  }

  public BotUser(User user)
  {
    this.Id = user.Id;
    this.LastName = user.LastName;
    this.FirstName = user.FirstName;
    this.Username= user.Username;
    this.FirstMeet = DateTime.Now;
  }
  
  public BotUser(long id, string username, string firstName, string lastName, string userLanguage)
  {
    this.Id = id;
    this.Username = username;
    this.FirstName = firstName;
    this.LastName = lastName;
    this.UserLanguage = userLanguage;
    this.FirstMeet = DateTime.Now;
  }
}