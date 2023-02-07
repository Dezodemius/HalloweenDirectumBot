using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCommon.Scenarios;

public abstract class AutoChatScenario : CommandScenario
{
  public override bool ExecuteStep(ITelegramBotClient telegramBotClient, Update update)
  {
    if (!this.steps.MoveNext())
      return false;

    this.CurrentStep = this.steps.Current;
    base.ExecuteStep(telegramBotClient, update);

    return true;
  }
}