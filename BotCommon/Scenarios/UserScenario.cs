using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCommon.Scenarios;

[PrimaryKey(nameof(UserId), nameof(ChatScenarioGuid))]
public class UserScenario
{
  [Required]
  public long UserId { get; set; }

  [Required]
  public Guid ChatScenarioGuid { get; set; }

  [NotMapped]
  public ChatScenario ChatScenario { get; set; }

  public bool Run(ITelegramBotClient bot, Update update)
  {
    return ChatScenario.RunNextStep(bot, update);
  }

  public UserScenario()
  {
  }

  public UserScenario(long userId, ChatScenario chatScenario)
  {
    this.UserId = userId;
    this.ChatScenario = chatScenario;
    this.ChatScenarioGuid = chatScenario.Id;
  }
}