using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCommon.Scenarios;

public abstract class AutoStepBotCommandScenario : BotCommandScenario
{
  public override async Task<bool> ExecuteStep(ITelegramBotClient telegramBotClient, Update update, long chatId)
  {
    if (!this.steps.MoveNext())
      return false;

    this.CurrentStep = this.steps.Current;
    return await base.ExecuteStep(telegramBotClient, update, chatId);
  }
}