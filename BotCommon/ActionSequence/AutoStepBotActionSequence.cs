using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCommon.ActionSequence;

/// <summary>
/// Auto step scenario.
/// </summary>
public abstract class AutoStepBotActionSequence 
  : BotActionSequence
{
  public override async Task<bool> ExecuteStep(ITelegramBotClient botClient, Update update, long chatId)
  {
    if (!sequenceActions.MoveNext())
      return false;

    CurrentAction = sequenceActions.Current;
    return await base.ExecuteStep(botClient, update, chatId);
  }
}