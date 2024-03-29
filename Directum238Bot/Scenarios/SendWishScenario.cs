using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BotCommon;
using BotCommon.Scenarios;
using Directum238Bot.Repository;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using File = System.IO.File;

namespace Directum238Bot.Scenarios;

public class SendWishScenario : AutoStepBotCommandScenario
{
  private readonly UserContentCache cache;
  private readonly string wishDay;
  public override Guid Id => new Guid("6A5102BE-668C-42D2-9FD2-818494DCDE8B");

  public override string ScenarioCommand => BotChatCommand.GetWish23;

  public static async Task SendInstruction(ITelegramBotClient botClient, Update update, long chatId)
  {
    var inlineMarkup = new InlineKeyboardMarkup(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(Directum238BotResources.IHaveWishMessage, BotChatCommand.UserWish)
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(Directum238BotResources.INeedBotHelpToWishMessage,
              BotChatCommand.GenerateWish)
        },
        new[] { InlineKeyboardButton.WithCallbackData(Directum238BotResources.GoStartMenu, BotChatCommand.MainMenu) }
    });
    if (update.CallbackQuery is { Message: { } })
    {
      await botClient.EditMessageTextAsync(chatId, update.CallbackQuery.Message.MessageId,
        Directum238BotResources.SendWishDescription, replyMarkup: inlineMarkup);
    }
  }

  public async Task ChooseWishGenerator(ITelegramBotClient botClient, Update update, long chatId)
  {
    var buttons = new List<InlineKeyboardButton[]>
    {
        new[] { InlineKeyboardButton.WithCallbackData(Directum238BotResources.GoStartMenu, BotChatCommand.MainMenu) }
    };
    if (update.Type == UpdateType.CallbackQuery)
    {
      switch (update.CallbackQuery.Data)
      {
        case BotChatCommand.GenerateWish:
        {
          string aiWish;
          await botClient.SendAnimationAsync(chatId, new InputOnlineFile(File.OpenRead(GetGifPath("6.gif")), "6.gif"));
          await botClient.SendTextMessageAsync(chatId,
            "Требуется немного времени. Нейросеть ChatGPT собирает самые лучшие слова для поздравления");
          try
          {
            aiWish = await GetAIWish();
            aiWish = $"{aiWish}{Environment.NewLine}{Environment.NewLine}by нейросеть и твои коллеги";
          }
          catch (Exception e)
          {
            var backToMenuMarkup = new InlineKeyboardMarkup(
              InlineKeyboardButton.WithCallbackData(Directum238BotResources.GoStartMenu, BotChatCommand.MainMenu));
            await botClient.SendTextMessageAsync(chatId,
              "Упс... бот тоже не смог придумать поздравление. Вся надежда на тебя", replyMarkup: backToMenuMarkup);
            LogManager.GetCurrentClassLogger().Error(e);
            return;
          }

          buttons.Insert(0,
            new[]
            {
                InlineKeyboardButton.WithCallbackData(Directum238BotResources.SendWishButton, BotChatCommand.Send)
            });
          var inlineMarkup = new InlineKeyboardMarkup(buttons);
          await botClient.SendTextMessageAsync(chatId, aiWish, replyMarkup: inlineMarkup);
          this.steps.MoveNext();
          break;
        }
        case BotChatCommand.UserWish:
        {
          var inlineMarkup = new InlineKeyboardMarkup(buttons);
          await botClient.EditMessageTextAsync(chatId, update.CallbackQuery.Message.MessageId,
            Directum238BotResources.SendWishToMe, replyMarkup: inlineMarkup);
          break;
        }
      }
    }
  }

  public async Task SendUserCheckMessage(ITelegramBotClient botClient, Update update, long chatId)
  {
    var inlineMarkup = new InlineKeyboardMarkup(new[]
    {
        new[] { InlineKeyboardButton.WithCallbackData(Directum238BotResources.SendWishButton, BotChatCommand.Send) },
        new[] { InlineKeyboardButton.WithCallbackData(Directum238BotResources.GoStartMenu, BotChatCommand.MainMenu) }
    });

    if (update.Type != UpdateType.Message || update.Message == null)
      return;

    var messageType = update.Message.Type;
    if (messageType != MessageType.Text && messageType != MessageType.VideoNote && messageType == MessageType.Voice)
    {
      await botClient.SendTextMessageAsync(chatId, 
        "Упс... Я не поддерживаю такие форматы поздравлений :( Попробуй отправить текст, голосовое, либо видео-кружок",
        replyMarkup: new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData(Directum238BotResources.GoStartMenu, BotChatCommand.MainMenu)));
      return;
    }
    await botClient.SendTextMessageAsync(chatId, Directum238BotResources.SendWishConfirmationMessage);
    await botClient.CopyMessageAsync(chatId, chatId, update.Message.MessageId, replyMarkup: inlineMarkup);
    await botClient.DeleteMessageAsync(chatId, update.Message.MessageId);
  }

  public async Task ConfirmSending(ITelegramBotClient botClient, Update update, long chatId)
  {
    if (update.CallbackQuery != null)
    {
      if (update.CallbackQuery.Data == BotChatCommand.Send)
      {
        var type = update.CallbackQuery.Message.Type;
        var inlineMarkup = new InlineKeyboardMarkup(new []
        {
          InlineKeyboardButton.WithCallbackData(Directum238BotResources.GoStartMenu, BotChatCommand.MainMenu)
        });
        string beforeWishDayDate;
        switch (wishDay)
        {
          case WishDay.Day23:
          {
            beforeWishDayDate = "22 февраля";
            break;
          }
          case WishDay.Day8:
          {
            beforeWishDayDate = "7 марта";
            break;
          }
          default:
          {
            beforeWishDayDate = "после того, как все приготовят поздравления";
            break;
          }
        }

        var gif = wishDay == WishDay.Day23
            ? new InputOnlineFile(File.OpenRead(GetGifPath("2.gif")), "2.gif")
            : new InputOnlineFile(File.OpenRead(GetGifPath("3.gif")), "3.gif");
        await botClient.SendAnimationAsync(chatId, gif);
        await botClient.SendTextMessageAsync(chatId,
          string.Format(Directum238BotResources.AfterMessageSaveMessage, beforeWishDayDate),
          replyMarkup: inlineMarkup);

        await botClient.DeleteMessageAsync(chatId, update.CallbackQuery.Message.MessageId);
        var content = type switch
        {
            MessageType.Text => update.CallbackQuery.Message.Text,
            MessageType.VideoNote => update.CallbackQuery.Message.VideoNote.FileId,
            MessageType.Voice => update.CallbackQuery.Message.Voice.FileId,
            _ => string.Empty
        };

        cache.Add(new UserContent(chatId, content, type, wishDay));
      }
    }
  }

  private static string GetGifPath(string gifFileName)
  {
    return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GIFs", gifFileName);
  }

  private async Task<string> GetAIWish()
  {
    var questionToAi = wishDay switch
    {
        WishDay.Day23 => "напиши поздравление для мужчин и женщин с 23 февраля. Можешь сделать это с юмором. Не используя слова армия, поле боя, государство и прочие плохие слова. Сделай это максимально умиротворённо.",
        WishDay.Day8 => "напиши поздравление для прекрасных дам с этим прекрасным весенним праздников, международным женским днём 8 марта. Можешь сделать это с юмором.",
        _ => string.Empty
    };
    return await new OpenAIClient(new BotConfigManager().Config.OpenAiApiKey).GetAnswer(questionToAi);
  }

  public SendWishScenario(UserContentCache cache, string wishDay)
  {
    this.cache = cache;
    this.wishDay = wishDay;
    this.steps = new List<BotCommandScenarioStep>
    {
        new (SendInstruction),
        new (ChooseWishGenerator),
        new (SendUserCheckMessage),
        new (ConfirmSending)
    }.GetEnumerator();
  }
}