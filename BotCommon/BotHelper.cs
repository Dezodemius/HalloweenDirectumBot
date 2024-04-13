using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BotCommon;

/// <summary>
/// Bot helper.
/// </summary>
public static class BotHelper
{
  /// <summary>
  /// Get user from update.
  /// </summary>
  /// <param name="update">Telegram bot update.</param>
  /// <returns>User.</returns>
  public static User GetUserFromUpdate(Update update)
  {
    return update.Type switch
      {
      UpdateType.Message => update.Message?.From,
          UpdateType.CallbackQuery => update.CallbackQuery?.From,
          _ => default
      };
  }
  
  /// <summary>
  /// Get telegram user username.
  /// </summary>
  /// <param name="update">Telegram update.</param>
  /// <returns>Username.</returns>
  public static string GetUsername(Update update)
  {
    return GetUsername(GetUserFromUpdate(update));
  }

  /// <summary>
  /// Get telegram user username.
  /// </summary>
  /// <param name="user">Telegram user.</param>
  /// <returns>Username.</returns>
  public static string GetUsername(User user)
  {
    return string.IsNullOrEmpty(user.Username)
      ? $"{user.FirstName} {user.LastName}"
      : user.Username;
  }
  
  /// <summary>
  /// Get message text from update.
  /// </summary>
  /// <param name="update">Telegram update.</param>
  /// <returns>Message text.</returns>
  public static string GetMessageText(Update update)
  {
    return update.Type switch
    {
      UpdateType.Message => update.Message?.Text,
      UpdateType.CallbackQuery => update.CallbackQuery?.Data,
      _ => null
    };
  }
}