using System;
using System.Collections.Generic;
using System.Linq;
using BotCommon.Repository;
using Microsoft.EntityFrameworkCore;

namespace BotCommon;

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


  public ActiveUsersManager(string connectionString) : base(connectionString)
  {
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