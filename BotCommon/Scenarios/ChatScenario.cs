using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Directum238Bot;

public class ChatScenario
{
  public Guid Id { get; set; }

  public ScenarioStep CurrentStep { get; set; }

  protected IEnumerator<ScenarioStep> steps;

  public bool RunNextStep(ITelegramBotClient telegramBotClient, Update update)
  {
    if (!this.steps.MoveNext())
      return false;

    this.CurrentStep = this.steps.Current;
    this.CurrentStep?.StepAction(telegramBotClient, update);

    return true;
  }

  public void Reset()
  {
    this.steps.Reset();
    this.CurrentStep = this.steps.Current;
  }

  public ChatScenario(Guid id)
  {
    this.Id = id;
  }
}