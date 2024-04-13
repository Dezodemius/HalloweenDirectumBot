using System.Collections.Generic;
using System.Linq;
using BotCommon.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace BotCommon.Repository;

/// <summary>
/// User DB context for EF.
/// </summary>
public class UserDbContext 
  : DefaultDbContext<TelegramUser>
{
  #region Constants

  private const string DefaultDbConnectionString = "Filename=app.db";

  #endregion

  #region Fields and Properties

  /// <summary>
  /// Telegram bot users.
  /// </summary>
  public DbSet<TelegramUser> Users { get; set; }

  #endregion

  #region Base class

  public override TelegramUser Get(TelegramUser user)
  {
    return Users.SingleOrDefault(u => u.Id == user.Id);
  }

  public override IEnumerable<TelegramUser> GetAll()
  {
    return Users;
  }

  public override void Add(TelegramUser user)
  {
    if (Get(user) != null)
      return;
    Users.Add(user);
    base.Add(user);
  }

  public override void Delete(TelegramUser user)
  {
    Users.Remove(user);
    base.Delete(user);
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseSqlite(_connectionString);
  }

  #endregion

  #region Constructors

  /// <summary>
  /// Constructor.
  /// </summary>
  public UserDbContext() 
    : this(DefaultDbConnectionString)
  {
    
  }
  
  /// <summary>
  /// Constructor.
  /// </summary>
  /// <param name="connectionString">Db connection string.</param>
  public UserDbContext(string connectionString) 
    : base(connectionString)
  {
  }

  #endregion
}