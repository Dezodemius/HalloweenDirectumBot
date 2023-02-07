using BotCommon.Scenarios;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Directum238Bot.Scenarios;

public class StartScenario : ChatScenario
{
  public override string ScenarioCommand => BotChatCommand.Start;

  public override Guid Id { get; set; } = new Guid("73645235-88E5-4132-9722-2FFE0269369B");

  private static async Task ShowStartMessage(ITelegramBotClient bot, Update update)
  {
    var chatId = update.Message.Chat.Id;
    await bot.SendTextMessageAsync(chatId, "Привет, pidor");
  }

  private static async Task ShowNextMessage(ITelegramBotClient bot, Update update)
  {
    var chatId = update.Message.Chat.Id;
    await bot.SendTextMessageAsync(chatId, $"ты написал, {update.Message.Text}");
  }

  public StartScenario()
  {
    this.steps = new List<ScenarioStep>
    {
      new ScenarioStep(1, ShowStartMessage),
      new ScenarioStep(2, ShowNextMessage)
    }.GetEnumerator();
  }
}