using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Directum238Bot;

public class BotCommandScenarioStep
{
  public delegate Task StepActionDelegate(ITelegramBotClient bot, Update update);

  public StepActionDelegate StepAction { get; }

  public BotCommandScenarioStep() { }

  public BotCommandScenarioStep(StepActionDelegate stepAction)
  {
    this.StepAction = stepAction;
  }
}