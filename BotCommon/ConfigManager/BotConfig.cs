namespace BotCommon.ConfigManager;

/// <summary>
/// Telegram bot configuration.
/// </summary>
public class BotConfig
{
  /// <summary>
  /// Bot Token.
  /// </summary>
  public string BotToken { get; set; }

  /// <summary>
  /// Bot admins.
  /// </summary>
  public long[] BotAdminId { get; set; }

  /// <summary>
  /// Database connection string.
  /// </summary>
  public string DbConnectionString { get; set; }
}