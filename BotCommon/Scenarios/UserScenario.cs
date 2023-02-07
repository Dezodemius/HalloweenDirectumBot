using System;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Directum238Bot;

public class UserScenario
{
  public long UserId { get; set; }

  // public UserScenarioStatus Status { get; set; }

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
  }
}

// public enum UserScenarioStatus
// {
//   Active,
//   Inactive
// }