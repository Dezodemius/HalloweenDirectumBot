using System;
using System.Collections.Generic;
using Directum238Bot;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCommon.Scenarios;

[PrimaryKey(nameof(Id))]
public class ChatScenario
{
  public virtual Guid Id { get; set; }

  public virtual string ScenarioCommand { get; }

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

  public ChatScenario()
  {
  }
}