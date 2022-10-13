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
        private static string[] prizeWords = new[]
        {
            "...сел медведь в машину и сгорел",
            "Два стоматолога поспорили кто больше выпьет из чаши...",
            "Пупа и Лупа пошли получать зарплату...",
            "Бу!",
            "1C",
            "Заходит богатый вампир в бар...",
            "Заходит вампир среднего достатка в бар...",
            "Заходит бедный вампир в бар...",
            "Гладиолус",
            "Хочешь узнать какой ты вампир?",
        };

        private const int NumberOfWinners = 10;

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
            var botClient = new TelegramBotClient(token);
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new[]
                {
                    UpdateType.Message,
                    UpdateType.CallbackQuery
                }
            };

            StickersManager.InitializeStickerPack(botClient, "MistressoftheDark");

            botClient.StartReceiving(HandleUpdate, HandleError, receiverOptions);

            Console.ReadLine();
        }

        private static async Task HandleUpdate(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
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
                    StickersManager.SendStickerAsync(bot, chatId, Emojis.Hello);
                    await bot.SendTextMessageAsync(
                        chatId,
                        $"Привет, {message.From.FirstName}!" +
                        $"\nВот ты и нашёл все баги. Чтобы узнать получишь ли ты приз нажми \"{BotCommands.Prize}\"." +
                        $"\nНо также ты можешь подобрать себе фильм для просмотра! Для этого нажми \"{BotCommands.Movie}\"",
                        replyMarkup: new ReplyKeyboardMarkup(new[]
                        {
                            new KeyboardButton(BotCommands.Prize),
                            new KeyboardButton(BotCommands.Movie)
                        }) { ResizeKeyboard = true },
                        cancellationToken: cancellationToken);
                    break;
                }
                case BotCommands.Prize:
                {
                    if (winners.Count < NumberOfWinners)
                    {
                        if (winners.TryGetValue(message.From.Id, out _))
                        {
                            StickersManager.SendStickerAsync(bot, chatId, Emojis.ExpressionlessFace);
                            await bot.SendTextMessageAsync(chatId,
                                $"Слышь, {message.From.FirstName}, у тебя уже есть подарок, иди отсюда!!!",
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
                                $"Поздравляю, {message.From.FirstName}! Ты один из счастливчиков, выигравших печенье от Эльвиры! Твой номер {number + 1} и твоя страшная фраза:\n \"{word}\"",
                                cancellationToken: cancellationToken);
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
                    break;
                }
                case BotCommands.Movie:
                {
                    const string MoviesDirectory = "movies";
                    const string PosterFilename = "poster.png";
                    const string MovieDescriptionFilename = "description";

                    var allMovies = Directory.GetDirectories(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, MoviesDirectory));
                    var randomMovieDirectory = allMovies[new Random(DateTime.Now.Millisecond).Next(0, allMovies.Length)];
                    var posterPath = Path.Combine(randomMovieDirectory, PosterFilename);
                    var description = File.ReadAllText(Path.Combine(randomMovieDirectory, MovieDescriptionFilename));
                    await using var stream = File.OpenRead(posterPath);
                    await bot.SendPhotoAsync(
                        chatId,
                        new InputOnlineFile(stream), description,
                        ParseMode.Markdown, cancellationToken: cancellationToken);
                    break;
                }
                default:
                {
                    StickersManager.SendStickerAsync(bot, chatId, Emojis.PensiveFace);;
                    await bot.SendTextMessageAsync(
                        chatId,
                        "Я не понимаю тебя, попробуй еще раз",
                        cancellationToken: cancellationToken);
                    break;
                }
            }
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
