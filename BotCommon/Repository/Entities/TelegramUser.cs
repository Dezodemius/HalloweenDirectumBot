using System;
using System.ComponentModel.DataAnnotations;
using Telegram.Bot.Types;

namespace BotCommon.Repository.Entities;

/// <summary>
/// Telegram bot user.
/// </summary>
public class TelegramUser
{
  #region Fields and Properties

  /// <summary>
  /// User Id.
  /// </summary>
  [Key]
  public long Id { get; set; }
  
  /// <summary>
  /// Telegram username.
  /// </summary>
  public string Username { get; set; }
  
  /// <summary>
  /// User first name.
  /// </summary>
  public string FirstName { get; set; }
  
  /// <summary>
  /// User last name.
  /// </summary>
  public string LastName { get; set; }
  
  /// <summary>
  /// User client language.
  /// </summary>
  public string UserLanguage { get; set; }
  
  /// <summary>
  /// User first meet in bot.
  /// </summary>
  public DateTime FirstMeet { get; set; }

  #endregion

  #region Constructors

  /// <summary>
  /// Constructor.
  /// </summary>
  public TelegramUser() { }

  /// <summary>
  /// Constructor.
  /// </summary>
  /// <param name="user">Telegram user.</param>
  public TelegramUser(User user)
  {
      Id = user.Id;
      LastName = user.LastName;
      FirstName = user.FirstName;
      Username= user.Username;
      FirstMeet = DateTime.Now;
  }

  /// <summary>
  /// Constructor.
  /// </summary>
  /// <param name="id">User Id.</param>
  /// <param name="username">User name.</param>
  /// <param name="firstName">User first name.</param>
  /// <param name="lastName">User last name.</param>
  /// <param name="userLanguage">User client language.</param>
  public TelegramUser(
      long id, 
      string username, 
      string firstName, 
      string lastName, 
      string userLanguage)
  {
      Id = id;
      Username = username;
      FirstName = firstName;
      LastName = lastName;
      UserLanguage = userLanguage;
      FirstMeet = DateTime.Now;
  }

  #endregion
}