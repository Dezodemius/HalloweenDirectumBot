﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BotCommon.Scenarios;
using Directum238Bot.Repository;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace Directum238Bot.Scenarios;

public class SendWishScenario : AutoStepBotCommandScenario
{
  private UserContentCache cache;
  private readonly string wishDay;
  public override Guid Id => new Guid("6A5102BE-668C-42D2-9FD2-818494DCDE8B");

  public override string ScenarioCommand => BotChatCommand.GetWish23;

  public static async Task SendInstruction(ITelegramBotClient botClient, Update update, long chatId)
  {
    var inlineMarkup = new InlineKeyboardMarkup(new []
    {
      InlineKeyboardButton.WithCallbackData("В главное меню ↩", BotChatCommand.Start)
    });
    await botClient.SendTextMessageAsync(chatId, "пришли мне поздравление (текст, голосовое, видеосообщение)", replyMarkup: inlineMarkup);
  }

  public static async Task SendUserCheckMessage(ITelegramBotClient botClient, Update update, long chatId)
  {
    if (update.Type != UpdateType.Message || update.Message == null)
      return;

    var inlineMarkup = new InlineKeyboardMarkup(new []
    {
      new [] { InlineKeyboardButton.WithCallbackData(Directum238BotResources.MessageSendConfirm, Directum238BotResources.MessageSendConfirm) },
      new [] { InlineKeyboardButton.WithCallbackData(Directum238BotResources.GoStartMenu, BotChatCommand.Start)}
    });
    switch (update.Message.Type)
    {
      case MessageType.VideoNote:
        if (update.Message.VideoNote != null)
          await botClient.SendVideoNoteAsync(chatId, new InputOnlineFile(update.Message.VideoNote.FileId),
            replyMarkup: inlineMarkup);
        break;
      case MessageType.Text:
        if (update.Message.Text != null)
          await botClient.SendTextMessageAsync(chatId, update.Message.Text, replyMarkup: inlineMarkup);
        break;
      case MessageType.Voice:
        if (update.Message.Voice != null)
          await botClient.SendVoiceAsync(chatId, new InputOnlineFile(update.Message.Voice.FileId),
            replyMarkup: inlineMarkup);
        break;
      default:
        await botClient.SendTextMessageAsync(chatId, Directum238BotResources.UnknownMessageType);
        break;
    }
    await botClient.DeleteMessageAsync(chatId, update.Message.MessageId);
  }

  public async Task ConfirmSending(ITelegramBotClient botClient, Update update, long chatId)
  {
    if (update.CallbackQuery != null)
    {
      if (update.CallbackQuery.Data == Directum238BotResources.MessageSendConfirm)
      {
        var type = update.CallbackQuery.Message.Type;
        var inlineMarkup = new InlineKeyboardMarkup(new []
        {
          InlineKeyboardButton.WithCallbackData(Directum238BotResources.GoStartMenu, BotChatCommand.Start)
        });
        await botClient.DeleteMessageAsync(chatId, update.CallbackQuery.Message.MessageId);
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

        await botClient.SendTextMessageAsync(chatId,
          string.Format(Directum238BotResources.AfterMessageSaveMessage, beforeWishDayDate), replyMarkup: inlineMarkup);

        switch (type)
        {
          case MessageType.Text:
          {
            cache.Add(new UserContent(chatId, update.CallbackQuery.Message.Text,
              update.CallbackQuery.Message.Type, wishDay));
            break;
          }
          case MessageType.VideoNote:
          {
            cache.Add(new UserContent(chatId, update.CallbackQuery.Message.VideoNote.FileId,
              update.CallbackQuery.Message.Type, wishDay));
            break;
          }
          case MessageType.Voice:
          {
            cache.Add(new UserContent(chatId, update.CallbackQuery.Message.Voice.FileId,
              update.CallbackQuery.Message.Type, wishDay));
            break;
          }
        }
      }
    }
  }

  public SendWishScenario(UserContentCache cache, string wishDay)
  {
    this.cache = cache;
    this.wishDay = wishDay;
    this.steps = new List<BotCommandScenarioStep>
    {
        new (SendInstruction),
        new (SendUserCheckMessage),
        new (ConfirmSending)
    }.GetEnumerator();
  }
}