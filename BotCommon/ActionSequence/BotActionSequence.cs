using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCommon.ActionSequence;

/// <summary>
/// Bot action sequence. 
/// </summary>
public abstract class BotActionSequence
{
  #region Fields and Properties

  /// <summary>
  /// Sequence Id.
  /// </summary>
  public abstract Guid Id { get; }

  /// <summary>
  /// Current running sequence element.
  /// </summary>
  public SequenceAction CurrentAction { get; set; }

  /// <summary>
  /// Sequence of actions.
  /// </summary>
  protected IEnumerator<SequenceAction> sequenceActions;

  #endregion

  #region Methods

  /// <summary>
  /// Execute sequence action.
  /// </summary>
  /// <param name="botClient">Telegram bot client.</param>
  /// <param name="update">Telegram update.</param>
  /// <param name="chatId">Telegram chat id.</param>
  /// <returns>Action executed flag.</returns>
  public virtual async Task<bool> ExecuteStep(ITelegramBotClient botClient, Update update, long chatId)
  {
    var canExecute = CurrentAction != null;
    if (canExecute)
      await CurrentAction.Action(botClient, update, chatId)!;
    return canExecute;
  }

  /// <summary>
  /// Reset sequence.
  /// </summary>
  public virtual void Reset()
  {
    sequenceActions.Reset();
    CurrentAction = sequenceActions.Current;
  }

  #endregion

  #region Constructors

  /// <summary>
  /// Constructor.
  /// </summary>
  public BotActionSequence() { }

  #endregion
}