using BotCommon.Scenarios;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace Directum238Bot.Scenarios;

public class Wish23Scenario : AutoStepBotCommandScenario
{
  public override Guid Id { get; set; } = new Guid("6A5102BE-668C-42D2-9FD2-818494DCDE8B");

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

    if (update.Message.VideoNote != null)
    {
      await botClient.SendVideoNoteAsync(chatId, new InputOnlineFile(update.Message.VideoNote.FileId));
    }
    else if (update.Message.Text != null)
      await botClient.SendTextMessageAsync(chatId, update.Message.Text);
    else if (update.Message.Voice != null)
    {
      await botClient.SendVoiceAsync(chatId, new InputOnlineFile(update.Message.Voice.FileId));
    }

    var inlineMarkup = new InlineKeyboardMarkup(new []
    {
      InlineKeyboardButton.WithCallbackData("Отправить", "Отправить"),
    });
    await botClient.SendTextMessageAsync(chatId, "подтверди", replyMarkup: inlineMarkup);
  }

  public async Task ConfirmSending(ITelegramBotClient botClient, Update update)
  {
    var chatId = update.CallbackQuery.From.Id;
    if (update.CallbackQuery != null && update.CallbackQuery.Data == "Отправить")
    {
      await botClient.DeleteMessageAsync(chatId, update.CallbackQuery.Message.MessageId);
      await botClient.SendTextMessageAsync(chatId, "sent");
    }
  }

  public Wish23Scenario()
  {
    this.steps = new List<BotCommandScenarioStep>
    {
        new (1, SendInstruction),
        new (2, SendUserCheckMessage),
        new (3, ConfirmSending)
    }.GetEnumerator();
  }
}