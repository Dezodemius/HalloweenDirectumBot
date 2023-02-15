using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BotCommon.Scenarios;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Directum238Bot.Scenarios;

public class GetWishScenario : AutoStepBotCommandScenario
{
  private readonly WishDay wishDay;
  private readonly UserContentCache cache;
  public override Guid Id => new Guid("3BFA76E9-D083-42D9-97E7-EAC8CE06A41D");
  public override string ScenarioCommand => string.Empty;

  public async Task GetWish(ITelegramBotClient botClient, Update update, long chatId)
  {
    var wish = this.cache.GetRandomContentExceptCurrent(chatId, this.wishDay);
    var markup = new InlineKeyboardMarkup(new []
    {
      InlineKeyboardButton.WithCallbackData("В главное меню ↩", BotChatCommand.Start)
    });
    switch (wish.Type)
    {
      case MessageType.Audio:
      {
        await botClient.SendAudioAsync(chatId, new InputMedia(wish.Content), replyMarkup: markup);
        break;
      }
      case MessageType.VideoNote:
      {
        await botClient.SendVideoAsync(chatId, new InputMedia(wish.Content), replyMarkup: markup);
        break;
      }
      case MessageType.Text:
      {
        await botClient.SendTextMessageAsync(chatId, wish.Content, replyMarkup: markup);
        break;
      }
    }
  }

  public GetWishScenario(UserContentCache cache, WishDay wishDay)
  {
    this.cache = cache;
    this.wishDay = wishDay;
    this.steps = new List<BotCommandScenarioStep>
    {
      new (GetWish)
    }.GetEnumerator();
  }
}