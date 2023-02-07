namespace BotCommon;

/// <summary>
/// Telegram bot configuration.
/// </summary>
public class BotConfig
{
  /// <summary>
  /// Bot Token.
  /// </summary>
  public string BotToken { get; set; }

  public string BotAdminId { get; set; }

  public string DbConnectionString { get; set; }
}