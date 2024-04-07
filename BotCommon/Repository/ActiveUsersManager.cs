using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BotCommon.Repository;

public sealed class ActiveUsersManager : DefaultDbContext<BotUser>
{
  private DbSet<BotUser> BotUsers { get; set; }

  public override BotUser Get(BotUser user)
  {
    return this.BotUsers.SingleOrDefault(u => u.BotUserId == user.BotUserId);
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

  public ActiveUsersManager() : this("Filename=user.db")
  {
    
  }
  
  public ActiveUsersManager(string connectionString) : base(connectionString)
  {
  }
}

[Table("BotUsers")]
public class BotUser
{
  public long BotUserId { get; set; }
  public string UserNickname { get; set; }
  public string UserFirstName { get; set; }
  public string UserSurname { get; set; }
  public string UserLanguage { get; set; }
  public DateTime UserFirstMeet { get; set; }

  public BotUser()
  {
  }

  public BotUser(long id, string userNickname, string userFirstName, string userSurname, string userLanguage)
  {
    this.BotUserId = id;
    this.UserNickname = userNickname;
    this.UserFirstName = userFirstName;
    this.UserSurname = userSurname;
    this.UserLanguage = userLanguage;
    this.UserFirstMeet = DateTime.Now;
  }
}