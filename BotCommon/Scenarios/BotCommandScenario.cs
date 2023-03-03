using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCommon.Scenarios;

[PrimaryKey(nameof(Id))]
public abstract class BotCommandScenario
{
  public abstract Guid Id { get; }

  public abstract string ScenarioCommand { get; }

  public BotCommandScenarioStep CurrentStep { get; set; }

  protected IEnumerator<BotCommandScenarioStep> steps;

  public virtual async Task<bool> ExecuteStep(ITelegramBotClient telegramBotClient, Update update, long chatId)
  {
    var canExecute = this.CurrentStep != null;
    if (canExecute)
      await this.CurrentStep.StepAction(telegramBotClient, update, chatId)!;
    return canExecute;
  }

  public virtual void Reset()
  {
    this.steps.Reset();
    this.CurrentStep = this.steps.Current;
  }

  public BotCommandScenario() { }
}