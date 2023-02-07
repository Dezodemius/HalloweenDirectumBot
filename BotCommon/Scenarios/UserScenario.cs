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
  public AutoChatScenario AutoChatScenario { get; set; }

  public bool Run(ITelegramBotClient bot, Update update)
  {
    return AutoChatScenario.ExecuteStep(bot, update);
  }

  public UserScenario()
  {
  }

  public UserScenario(long userId, AutoChatScenario autoChatScenario)
  {
    this.UserId = userId;
    this.AutoChatScenario = autoChatScenario;
    this.ChatScenarioGuid = autoChatScenario.Id;
  }
}