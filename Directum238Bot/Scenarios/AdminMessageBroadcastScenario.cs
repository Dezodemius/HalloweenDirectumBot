using BotCommon;
using BotCommon.Scenarios;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace Directum238Bot.Scenarios;

public class AdminMessageBroadcastScenario : BotCommandScenario
{
  private readonly ActiveUsersManager cache;
  public override Guid Id { get; set; } = new Guid("8F2DD0DC-E84B-4319-8277-58222A671FC5");

  public override string ScenarioCommand { get; }


  private async Task BroadcastMessageCheck(ITelegramBotClient bot, Update update)
  {
    var chatId = update.Message.From.Id;
    var keyboard = new InlineKeyboardMarkup(new []
    {
      InlineKeyboardButton.WithCallbackData("Отправить", "Отправить"),
      InlineKeyboardButton.WithCallbackData("Удалить", "Удалить"),
    });
    switch (update.Message.Type)
    {
      case MessageType.Voice:
      {
        await bot.SendVoiceAsync(chatId, new InputOnlineFile(update.Message.Voice.FileId), replyMarkup: keyboard);
        break;
      }
      case MessageType.VideoNote:
      {
        await bot.SendVideoNoteAsync(chatId, new InputOnlineFile(update.Message.VideoNote.FileId), replyMarkup: keyboard);
        break;
      }
      case MessageType.Text:
      {
        await bot.SendTextMessageAsync(chatId, update.Message.Text, replyMarkup: keyboard);
        break;
      }
    }
    await bot.DeleteMessageAsync(chatId, update.Message.MessageId);
    this.steps.MoveNext();
    this.CurrentStep = steps.Current;
  }

  private async Task CheckMessage(ITelegramBotClient bot, Update update)
  {
    var chatId = update.CallbackQuery.From.Id;
    switch (update.CallbackQuery.Data)
    {
      case "Отправить":
      {
        var allUsers = cache.BotUsers.Select(c => c);
        foreach (var user in allUsers)
        {
          switch (update.CallbackQuery.Message.Type)
          {
            case MessageType.Voice:
            {
              await bot.SendVoiceAsync(user.BotUserId, new InputOnlineFile(update.CallbackQuery.Message.Voice.FileId));
              break;
            }
            case MessageType.VideoNote:
            {
              await bot.SendVideoNoteAsync(user.BotUserId, new InputOnlineFile(update.CallbackQuery.Message.VideoNote.FileId));
              break;
            }
            case MessageType.Text:
            {
              await bot.SendTextMessageAsync(user.BotUserId, update.CallbackQuery.Message.Text);
              break;
            }
          }
        }
        break;
      }
      case "Удалить":
      {
        await bot.DeleteMessageAsync(chatId, update.CallbackQuery.Message.MessageId);
        break;
      }
    }
    this.steps.Reset();
  }

  public AdminMessageBroadcastScenario(ActiveUsersManager cache)
  {
    this.cache = cache;
    this.steps = new List<BotCommandScenarioStep>
    {
        new (BroadcastMessageCheck),
        new (CheckMessage)
    }.GetEnumerator();
    steps.MoveNext();
    this.CurrentStep = steps.Current;
  }
}