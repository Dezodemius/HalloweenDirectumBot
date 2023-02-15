using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BotCommon.Scenarios;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Directum238Bot.Scenarios;

public class StartScenario : AutoStepBotCommandScenario
{
  public override Guid Id => new Guid("73645235-88E5-4132-9722-2FFE0269369B");

  public override string ScenarioCommand => BotChatCommand.Start;

  private static readonly DateTime day23WishGetTime = new DateTime(2023, 2, 22);
  private static readonly DateTime day8WishGetTime = new DateTime(2023, 3, 7);
  private static readonly DateTime day8WishSendTime = new DateTime(2023, 3, 1);

  private static async Task ShowStartMessage(ITelegramBotClient bot, Update update, long chatId)
  {
    var inlineButtons = new List<InlineKeyboardButton[]>
    {
      new []{InlineKeyboardButton.WithCallbackData("Поздравить с 23 февраля", BotChatCommand.SendWish23)},
    };
    if (DateTime.Now.CompareTo(day23WishGetTime) >= 0)
      inlineButtons.Add(new [] {InlineKeyboardButton.WithCallbackData("Получить поздравление с 23 февраля", BotChatCommand.GetWish23)});
    if (DateTime.Now.CompareTo(day8WishSendTime) >= 0)
      inlineButtons.Add(new [] { InlineKeyboardButton.WithCallbackData("Поздравить с 8 марта", BotChatCommand.SendWish8) });
    if (DateTime.Now.CompareTo(day8WishGetTime) >= 0)
      inlineButtons.Add(new [] { InlineKeyboardButton.WithCallbackData("Получить поздравление с 8 марта", BotChatCommand.GetWish8) });
    var markup = new InlineKeyboardMarkup(inlineButtons);
    await bot.SendTextMessageAsync(chatId, $"Привет!", replyMarkup: markup);
  }

  public StartScenario()
  {
    this.steps = new List<BotCommandScenarioStep>
    {
      new BotCommandScenarioStep(ShowStartMessage),
    }.GetEnumerator();
  }
}