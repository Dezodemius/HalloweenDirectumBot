using System.Collections.Concurrent;
using System.Text;
using HalloweenDirectumBot;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using File = System.IO.File;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace HalloweenDirectumBot
{
  public static class Program
  {
    private static MovieManager _movieManager = new ();

    private static string[] prizeWords = new[]
    {
      "Все твои сообщения руководителю будут писаться только капслоком",
      "Как бы ты не старался все твои задачи в Ауре будут красными",
      "Все печенье сегодня будет съедено другими. Ты даже крошек не увидишь",
      "У меня нет времени читать баг-репорт, опиши проблему в двух словах",
      "Зачем мне Directum, если я это всё и в Excel могу",
      "Чтоб у тебя RDP отвалился!",
      "[Просто нажми](www.youtube.com/watch?v=dQw4w9WgXcQ)",
      "Мы знаем, что ты себя плохо ведешь, поэтому твой Сhrome превратится в Internet Explorer",
      "Опять сборка не прошла? Ну давай, заплачь!",
      "Кодишь на C#? Ну давай, попробуй на C++",
      "Пишешь на Python? Ну давай, попробуй на C#",
      "Я знаю, что ты преувеличиваешь, когда ставишь отметку в Ауре!",
      "На твоём этаже самый невкусный кофе!",
    };

    private const int NumberOfWinners = 13;

    private static ConcurrentDictionary<long, int> winners = GetWinners();

    #region Методы

    public static void Main(string[] args)
    {
      AppDomain.CurrentDomain.ProcessExit += (sender, args) =>
      {
        using var stream = File.OpenWrite("winners.json");
        var json = JsonSerializer.Serialize(winners);
        stream.Write(Encoding.UTF8.GetBytes(json));
      };

      var token = args[0];
      string? exitInput;
      do
      {
        try
        {
          var botClient = new TelegramBotClient(token);
          var receiverOptions = new ReceiverOptions
          {
            AllowedUpdates = new[]
            {
              UpdateType.Message
            }
          };

          StickersManager.InitializeStickerPack(botClient, "MistressoftheDark");

          botClient.StartReceiving(HandleUpdate, HandleError, receiverOptions);
        }
        catch (Exception e)
        {
          Console.WriteLine(e);
        }

        exitInput = Console.ReadLine();
      } while (exitInput != null && exitInput != "exit");
    }

    private static async Task HandleUpdate(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
      switch (update.Type)
      {
        case UpdateType.Message:
        {
          await ProcessMessageUpdate(bot, update, cancellationToken);
          break;
        }
        default:
          Console.WriteLine("IGNORE UPDATE");
          return;
      }
    }

    private static async Task ProcessMessageUpdate(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {

      var message = update.Message;
      if (message is not { Type: MessageType.Text } || message.From is null)
        return;
      var chatId = message.Chat.Id;
      Console.WriteLine(JsonConvert.SerializeObject(message));
      switch (message.Text)
      {
        case BotCommands.Start:
        {
          await SendWelcomeMessage(bot, cancellationToken, chatId, message);
          break;
        }
        case BotCommands.Prize:
        {
          await GivePrizeIfNeeded(bot, cancellationToken, message, chatId);
          break;
        }
        case BotCommands.Movie:
        {
          await ChooseMovie(bot, cancellationToken, chatId);
          break;
        }
        default:
        {
          StickersManager.SendStickerAsync(bot, chatId, Emojis.PensiveFace);
          ;
          await bot.SendTextMessageAsync(
            chatId,
            "Я не понимаю тебя, попробуй еще раз",
            cancellationToken: cancellationToken);
          break;
        }
      }
    }

    private static async Task ChooseMovie(ITelegramBotClient bot, CancellationToken cancellationToken, long chatId)
    {
      var movie = _movieManager.GetNextMovie();
      await using (movie.MoviePosterPathStream)
      {
        await bot.SendPhotoAsync(
          chatId: chatId,
          photo: new InputOnlineFile(movie.MoviePosterPathStream),
          caption: movie.Description,
          parseMode: ParseMode.Markdown,
          cancellationToken: cancellationToken);
      }
    }

    private static async Task GivePrizeIfNeeded(ITelegramBotClient bot, CancellationToken cancellationToken, Message message, long chatId)
    {
      if (winners.Count < NumberOfWinners)
      {
        if (winners.TryGetValue(message.From.Id, out _))
        {
          StickersManager.SendStickerAsync(bot, chatId, Emojis.ExpressionlessFace);
          await bot.SendTextMessageAsync(chatId,
            $"Не пытайся меня обмануть. У тебя уже есть подарок.{Environment.NewLine}" +
            $"Все-го хо-ро-ше-го",
            cancellationToken: cancellationToken);
        }
        else
        {
          var lastWinner = winners.LastOrDefault();
          var number = lastWinner.Key == default ? 0 : lastWinner.Value + 1;
          var word = prizeWords[number];

          winners[message.From.Id] = number;
          StickersManager.SendStickerAsync(bot, chatId, Emojis.GrinningFaceWithBigEyes);
          await bot.SendTextMessageAsync(
            chatId,
            $"Поздравляю, ты один из счастливчиков, выигравших сладкий приз от компании.{Environment.NewLine}" +
            $"Скорее пиши [Эльвире Ибрагимовой](https://talk.directum.ru/directum/messages/@Ibragimova_ES) в ММ свой номер и фразу!{Environment.NewLine}{Environment.NewLine}" +
            $"Номер: {number + 1}{Environment.NewLine}" +
            $"Злобная фраза:\"{word}\"{Environment.NewLine}",
            ParseMode.Markdown,
            cancellationToken: cancellationToken);
          Console.WriteLine($"WINNER: {number} {(string.IsNullOrEmpty(message.From.Username) ? $"{message.From.FirstName} {message.From.LastName}" : message.From.Username)}");
        }
      }
      else
      {
        await bot.SendTextMessageAsync(
          chatId,
          "К сожалению, все призы уже разыграны. Приходи в следующем году!",
          cancellationToken: cancellationToken);
        StickersManager.SendStickerAsync(bot, chatId, Emojis.BlowingKiss);
      }
    }

    private static async Task SendWelcomeMessage(ITelegramBotClient bot, CancellationToken cancellationToken, long chatId, Message message)
    {
      StickersManager.SendStickerAsync(bot, chatId, Emojis.Hello);
      await bot.SendTextMessageAsync(
        chatId,
        $"Привет, {message.From.FirstName}!" +
        $"\nВот и найдены все баги. Чтобы узнать получишь ли ты приз нажми \"{BotCommands.Prize}\"." +
        $"\nНо также ты можешь подобрать себе фильм для просмотра! Для этого нажми \"{BotCommands.Movie}\"",
        replyMarkup: new ReplyKeyboardMarkup(new[]
        {
          new KeyboardButton(BotCommands.Prize),
          new KeyboardButton(BotCommands.Movie)
        }) { ResizeKeyboard = true },
        cancellationToken: cancellationToken);
    }

    private static async Task HandleError(ITelegramBotClient bot, Exception exception, CancellationToken cancellationToken)
    {
      Console.WriteLine(exception);
      Environment.Exit(1);
    }

    private static ConcurrentDictionary<long, int> GetWinners()
    {
      const string winnersFile = "winners.json";
      var winners = new ConcurrentDictionary<long, int>();
      if (File.Exists(winnersFile))
      {
        using var stream = File.OpenRead(winnersFile);
        winners = JsonSerializer.Deserialize<ConcurrentDictionary<long, int>>(stream)
                  ?? new ConcurrentDictionary<long, int>();
      }
      return winners;
    }

      #endregion
  }
}
