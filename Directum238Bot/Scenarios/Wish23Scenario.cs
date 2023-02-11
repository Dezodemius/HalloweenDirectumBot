using BotCommon.Scenarios;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace Directum238Bot.Scenarios;

public class Wish23Scenario : AutoStepBotCommandScenario
{
  private UserContentCache cache;
  public override Guid Id => new Guid("6A5102BE-668C-42D2-9FD2-818494DCDE8B");

  public override string ScenarioCommand => BotChatCommand.Wish23;

  public static async Task SendInstruction(ITelegramBotClient botClient, Update update)
  {
    var chatId = update.Message.Chat.Id;
    await botClient.SendTextMessageAsync(chatId, "пришли мне что-то");
  }

  public static async Task SendUserCheckMessage(ITelegramBotClient botClient, Update update)
  {
    if (update.Type != UpdateType.Message)
      return;
    var chatId = update.Message.Chat.Id;

    var inlineMarkup = new InlineKeyboardMarkup(new []
    {
      InlineKeyboardButton.WithCallbackData("Отправить", "Отправить"),
      InlineKeyboardButton.WithCallbackData("Удалить", "Удалить"),
    });
    switch (update.Message.Type)
    {
      case MessageType.VideoNote:
        await botClient.SendVideoNoteAsync(chatId, new InputOnlineFile(update.Message.VideoNote.FileId), replyMarkup: inlineMarkup);
        break;
      case MessageType.Text:
        await botClient.SendTextMessageAsync(chatId, update.Message.Text, replyMarkup: inlineMarkup);
        break;
      case MessageType.Voice:
        await botClient.SendVoiceAsync(chatId, new InputOnlineFile(update.Message.Voice.FileId), replyMarkup: inlineMarkup);
        break;
      default:
        await botClient.SendTextMessageAsync(chatId, "Poshel nahui");
        break;
    }
    await botClient.DeleteMessageAsync(chatId, update.Message.MessageId);
  }

  public async Task ConfirmSending(ITelegramBotClient botClient, Update update)
  {
    if (update.CallbackQuery != null)
    {
      var chatId = update.CallbackQuery.From.Id;
      switch (update.CallbackQuery.Data)
      {
        case "Отправить":
        {
          var type = update.CallbackQuery.Message.Type;
          await botClient.DeleteMessageAsync(chatId, update.CallbackQuery.Message.MessageId);
          await botClient.SendTextMessageAsync(chatId, "sent");

          var content = cache.GetRandomContentExceptCurrent(chatId, type);
          if (content == null)
            await botClient.SendTextMessageAsync(chatId, "no content");
          switch (type)
          {
            case MessageType.Text:
            {
              cache.UserContents.Add(new UserContent(chatId, update.CallbackQuery.Message.Text,
                update.CallbackQuery.Message.Type));
              if (content != null)
                await botClient.SendTextMessageAsync(chatId, content.Content);
              break;
            }
            case MessageType.VideoNote:
            {
              cache.UserContents.Add(new UserContent(chatId, update.CallbackQuery.Message.VideoNote.FileId,
                update.CallbackQuery.Message.Type));
              if (content != null)
                await botClient.SendVideoNoteAsync(chatId, new InputOnlineFile(content.Content));
              break;
            }
            case MessageType.Voice:
            {
              cache.UserContents.Add(new UserContent(chatId, update.CallbackQuery.Message.Voice.FileId,
                update.CallbackQuery.Message.Type));
              if (content != null)
                await botClient.SendVoiceAsync(chatId, new InputOnlineFile(content.Content));
              break;
            }
          }
          break;
        }
        case "Удалить":
        {
          await botClient.DeleteMessageAsync(chatId, update.CallbackQuery.Message.MessageId);
          await botClient.SendTextMessageAsync(chatId, "deleted");
          break;
        }
      }
        await cache.SaveChangesAsync();
    }
  }

  public Wish23Scenario(UserContentCache cache)
  {
    this.cache = cache;
    this.steps = new List<BotCommandScenarioStep>
    {
        new (SendInstruction),
        new (SendUserCheckMessage),
        new (ConfirmSending)
    }.GetEnumerator();
  }
}