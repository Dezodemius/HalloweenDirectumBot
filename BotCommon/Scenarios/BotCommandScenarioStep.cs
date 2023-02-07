using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Directum238Bot;

public class BotCommandScenarioStep
{
  public int Id { get; }

  public delegate Task StepActionDelegate(ITelegramBotClient bot, Update update);

  public StepActionDelegate StepAction { get; }

  public BotCommandScenarioStep() { }

  public BotCommandScenarioStep(int id, StepActionDelegate stepAction)
  {
    this.Id = id;
    this.StepAction = stepAction;
  }
}