using System;
using System.Collections.Generic;
using Directum238Bot;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCommon.Scenarios;

[PrimaryKey(nameof(Id))]
public abstract class CommandScenario
{
  public abstract Guid Id { get; set; }

  public abstract string ScenarioCommand { get; }

  public ScenarioStep CurrentStep { get; set; }

  protected IEnumerator<ScenarioStep> steps;

  public virtual bool ExecuteStep(ITelegramBotClient telegramBotClient, Update update)
  {
    this.CurrentStep?.StepAction(telegramBotClient, update);
    return true;
  }

  public virtual void Reset()
  {
    this.steps.Reset();
    this.CurrentStep = this.steps.Current;
  }

  public CommandScenario() { }
}