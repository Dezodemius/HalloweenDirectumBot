using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BotCommon.Scenarios;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace Directum238Bot.Scenarios;

public class StartScenario : AutoStepBotCommandScenario
{
  public override Guid Id => new Guid("73645235-88E5-4132-9722-2FFE0269369B");

  public override string ScenarioCommand => BotChatCommand.Start;

  private static readonly DateTime day23WishGetTime = new DateTime(2023, 2, 22);
  private static readonly DateTime day8WishGetTime = new DateTime(2023, 3, 7);
  private static readonly DateTime day8WishSendTime = new DateTime(2023, 3, 3);

  private static async Task ShowStartMessage(ITelegramBotClient bot, Update update, long chatId)
  {
    var inlineButtons = new List<InlineKeyboardButton[]>
    {
      new []{InlineKeyboardButton.WithCallbackData(Directum238BotResources.SendWish23, BotChatCommand.SendWish23)},
    };
    if (DateTime.Now.CompareTo(day23WishGetTime) >= 0)
      inlineButtons.Add(new [] {InlineKeyboardButton.WithCallbackData(Directum238BotResources.GetWish23, BotChatCommand.GetWish23)});
    if (DateTime.Now.CompareTo(day8WishSendTime) >= 0)
      inlineButtons.Add(new [] { InlineKeyboardButton.WithCallbackData(Directum238BotResources.SendWish8, BotChatCommand.SendWish8) });
    if (DateTime.Now.CompareTo(day8WishGetTime) >= 0)
      inlineButtons.Add(new [] { InlineKeyboardButton.WithCallbackData(Directum238BotResources.GetWish8, BotChatCommand.GetWish8) });
    var markup = new InlineKeyboardMarkup(inlineButtons);
    var gif = new InputOnlineFile(System.IO.File.OpenRead(GetGifPath("1.gif")), "hello.gif");
    await bot.SendAnimationAsync(chatId,
      gif,
      caption: Directum238BotResources.BotStartMessage,
      replyMarkup: markup,
      parseMode: ParseMode.Markdown);
  }

  private static string GetGifPath(string gifFileName)
  {
    return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GIFs", gifFileName);
  }

  public StartScenario()
  {
    this.steps = new List<BotCommandScenarioStep>
    {
      new BotCommandScenarioStep(ShowStartMessage),
    }.GetEnumerator();
  }
}