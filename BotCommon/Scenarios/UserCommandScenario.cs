using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCommon.Scenarios;

[PrimaryKey(nameof(UserId), nameof(ChatScenarioGuid))]
public class UserCommandScenario
{
  [Required]
  public long UserId { get; set; }

  [Required]
  public Guid ChatScenarioGuid { get; set; }

  [NotMapped]
  public BotCommandScenario CommandScenario { get; set; }

  public bool Run(ITelegramBotClient bot, Update update)
  {
    return CommandScenario.ExecuteStep(bot, update);
  }

  public UserCommandScenario() { }

  public UserCommandScenario(long userId, BotCommandScenario commandScenario)
  {
    this.UserId = userId;
    this.CommandScenario = commandScenario;
    this.ChatScenarioGuid = commandScenario.Id;
  }
}