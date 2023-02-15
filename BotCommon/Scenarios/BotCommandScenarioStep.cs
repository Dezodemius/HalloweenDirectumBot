using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCommon.Scenarios;

public class BotCommandScenarioStep
{
  public delegate Task StepActionDelegate(ITelegramBotClient bot, Update update, long chatId);

  public StepActionDelegate StepAction { get; }

  public BotCommandScenarioStep() { }

  public BotCommandScenarioStep(StepActionDelegate stepAction)
  {
    this.StepAction = stepAction;
  }
}