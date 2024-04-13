using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCommon.ActionSequence;

/// <summary>
/// Sequence action e executor.
/// </summary>
public class SequenceActionExecutor
{
  #region Fields and Properties

  /// <summary>
  /// User Id.
  /// </summary>
  public long UserId { get; set; }

  /// <summary>
  /// User sequence Id.
  /// </summary>
  public Guid UserSequenceId { get; set; }

  /// <summary>
  /// User sequence.
  /// </summary>
  public BotActionSequence ActionSequence { get; set; }

  #endregion

  #region Methods

  /// <summary>
  /// Execute sequence action.
  /// </summary>
  /// <param name="bot">Telegram bot client.</param>
  /// <param name="update">Telegram update.</param>
  /// <param name="userId">Telegram user Id.</param>
  /// <returns>Flag of action executed.</returns>
  public async Task<bool> ExecuteAction(ITelegramBotClient bot, Update update, long userId)
  {
    return await ActionSequence.ExecuteStep(bot, update, userId);
  }

  #endregion

  #region Constructors

  /// <summary>
  /// Constructor.
  /// </summary>
  public SequenceActionExecutor() { }

  /// <summary>
  /// Constructor.
  /// </summary>
  /// <param name="userId">Telegram user Id.</param>
  /// <param name="actionSequence">Action sequence.</param>
  public SequenceActionExecutor(long userId, BotActionSequence actionSequence)
  {
    UserId = userId;
    ActionSequence = actionSequence;
    UserSequenceId = actionSequence.Id;
  }

  #endregion
}