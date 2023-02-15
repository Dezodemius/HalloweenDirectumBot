using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCommon.Scenarios;

public abstract class AutoStepBotCommandScenario : BotCommandScenario
{
  public override bool ExecuteStep(ITelegramBotClient telegramBotClient, Update update, long chatId)
  {
    if (!this.steps.MoveNext())
      return false;

    this.CurrentStep = this.steps.Current;
    return base.ExecuteStep(telegramBotClient, update, chatId);
  }
}