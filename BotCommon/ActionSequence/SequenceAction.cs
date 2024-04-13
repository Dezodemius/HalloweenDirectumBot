using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCommon.ActionSequence;

/// <summary>
/// Bot action.
/// </summary>
public class SequenceAction
{
  /// <summary>
  /// Action delegate.
  /// </summary>
  public delegate Task ElementActionDelegate(ITelegramBotClient bot, Update update, long chatId);

  /// <summary>
  /// Action.
  /// </summary>
  public ElementActionDelegate Action { get; }

  /// <summary>
  /// Constructor.
  /// </summary>
  public SequenceAction() { }

  /// <summary>
  /// Constructor.
  /// </summary>
  /// <param name="action">Action.</param>
  public SequenceAction(ElementActionDelegate action)
  {
    Action = action;
  }
}