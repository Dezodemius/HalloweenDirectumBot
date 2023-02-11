using System;
using System.Collections.Generic;
using Directum238Bot;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCommon.Scenarios;

[PrimaryKey(nameof(Id))]
public abstract class BotCommandScenario
{
  public abstract Guid Id { get; set; }

  public abstract string ScenarioCommand { get; }

  public BotCommandScenarioStep CurrentStep { get; set; }

  protected IEnumerator<BotCommandScenarioStep> steps;

  public virtual bool ExecuteStep(ITelegramBotClient telegramBotClient, Update update)
  {
    var canExecute = this.CurrentStep != null;
    if (canExecute)
      this.CurrentStep?.StepAction(telegramBotClient, update);
    return canExecute;
  }

  public virtual void Reset()
  {
    this.steps.Reset();
    this.CurrentStep = this.steps.Current;
  }

  public BotCommandScenario() { }
}