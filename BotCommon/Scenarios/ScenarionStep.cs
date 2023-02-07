using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Directum238Bot;

public class ScenarioStep
{
  public int Id { get; }

  public delegate Task StepActionDelegate(ITelegramBotClient bot, Update update);

  public StepActionDelegate StepAction { get; }

  public ScenarioStep() { }

  public ScenarioStep(int id, StepActionDelegate stepAction)
  {
    this.Id = id;
    this.StepAction = stepAction;
  }
}