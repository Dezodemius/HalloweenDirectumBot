using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using BotCommon.Repository;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types.Enums;

namespace Directum238Bot;

public sealed class UserContentCache : DefaultDbContext<UserContent>
{
  private DbSet<UserContent> UserContents { get; set; }

  public UserContent GetRandomContentExceptCurrent(long userId, WishDay wishDay)
  {
    var founded = UserContents.Where(c => c.UserId != userId && c.WishDay == wishDay);
    var randomIndex = new Random().Next(founded.Count());
    return founded.ToList().ElementAtOrDefault(randomIndex);
  }


  public override UserContent Get(UserContent entity)
  {
    return this.UserContents.SingleOrDefault(c => c.Id == entity.Id);
  }

  public override IEnumerable<UserContent> GetAll()
  {
    return this.UserContents;
  }

  public override void Add(UserContent entity)
  {
    this.UserContents.Add(entity);
    base.Add(entity);
  }

  public override void Delete(UserContent entity)
  {
    this.UserContents.Remove(this.Get(entity));
    base.Delete(entity);
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseSqlite(this._connectionString);
  }

  public UserContentCache(string connectionString) : base(connectionString)
  {
  }
}

[Table("UserContents")]
[PrimaryKey(nameof(Id))]
public class UserContent
{
  public Guid Id { get; set; }
  public long UserId { get; set; }
  public string Content { get; set; }
  public MessageType Type { get; set; }
  public WishDay WishDay { get; set; }

  public UserContent() { }

  public UserContent(long userId, string content, MessageType type, WishDay wishDay)
  {
    this.Id  = new Guid();
    this.UserId = userId;
    this.Content = content;
    this.Type = type;
    this.WishDay = wishDay;
  }
}

public enum WishDay
{
  Day23,
  Day8
}