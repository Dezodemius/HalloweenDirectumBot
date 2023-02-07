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

  /// <summary>
  /// Bot name.
  /// </summary>
  public string BotName { get; set; }

  /// <summary>
  /// Bot info.
  /// </summary>
  public string BotAbout { get; set; }

  /// <summary>
  /// Bot description.
  /// </summary>
  public string BotDescription { get; set; }

  /// <summary>
  /// Bot description full filepath.
  /// </summary>
  public string BotDescriptionPictureFullFilepath { get; set; }

  /// <summary>
  /// Bot picture full filepath.
  /// </summary>
  public string BotPictureFullFilepath { get; set; }

  /// <summary>
  /// Bot commands list..
  /// </summary>
  public string BotCommands { get; set; }
}