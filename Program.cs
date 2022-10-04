using System.Text;
using HalloweenDirectumBot;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using File = System.IO.File;
using JsonSerializer = System.Text.Json.JsonSerializer;

var matchWord = new[]
{
    "Какая-то фраза",
    "Два стоматолога поспорили кто больше выпьет из чаши...",
    "Пупа и Лупа пошли получать зарплату...",
    "Бу!",
    "1C",
    "Какая-то фраза1",
    "Какая-то фраза2",
    "Какая-то фраза3",
    "Какая-то фраза4",
    "Какая-то фраза5",
};

const string winnersFile = "winners.json";
List<WinnersWithWord> winners;
if (File.Exists(winnersFile))
{
    await using var stream = File.OpenRead("winners.json");
    winners = JsonSerializer.Deserialize<List<WinnersWithWord>>(stream);
}
else
{
    winners = new List<WinnersWithWord>();
}

var botClient = new TelegramBotClient("5738215866:AAGdVlZD7HGC7gYyR-ee_9rQHsrDPJfW2a4");
var receiverOptions = new ReceiverOptions { AllowedUpdates = { } };
var stickers = await botClient.GetStickerSetAsync("MistressoftheDark");

botClient.StartReceiving(HandleUpdate, HandleError, receiverOptions);
AppDomain.CurrentDomain.ProcessExit += (sender, args) =>
{
    using var stream = File.OpenWrite("winners.json");
    var json = JsonSerializer.Serialize(winners);
    stream.Write(Encoding.UTF8.GetBytes(json));
};
Console.ReadLine();

async Task HandleUpdate(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
{
    if (update.Type == UpdateType.Message)
    {
        var message = update.Message;
        if (message is { Type: MessageType.Text })
        {
            Console.WriteLine(JsonConvert.SerializeObject(message));
            switch (message.Text)
            {
                case BotCommands.Start:
                    await bot.SendStickerAsync(message.Chat.Id, stickers.Stickers.Where(s => s.Emoji == "👋").First().FileId);
                    await bot.SendTextMessageAsync(message.Chat.Id, $"Привет, {message.From.FirstName}!\nВот ты и нашёл все баги. Чтобы узнать получишь ли ты приз нажми /prize.\nНо также ты можешь подобрать себе фильм для просмотра! Для этого нажми /movie");
                    break;

                case BotCommands.Prize:
                    lock (winners)
                    {
                        if (winners.Count < 10)
                        {
                            var lastWinner = winners.LastOrDefault();
                            var number = lastWinner == null ? 0 : winners.IndexOf(lastWinner) + 1;
                            var word = matchWord[number];
                            var username = message.Chat.Username ?? "никнейма нет :(";
                            var firstName = message.Chat.FirstName ?? "имени нет :(";
                            var newWinner = new WinnersWithWord(message.From.Id, number, firstName, username, word);

                            if (winners.SingleOrDefault(w => w.Nickname == newWinner.Nickname) != null)
                            {
                                bot.SendStickerAsync(message.Chat.Id, stickers.Stickers.Where(s => s.Emoji == "😑").First().FileId);
                                bot.SendTextMessageAsync(message.Chat.Id,
                                    $"Слышь, {firstName}, у тебя уже есть подарок, иди отсюда!!!",
                                    cancellationToken: cancellationToken);
                                return;
                            }

                            winners.Add(newWinner);
                            bot.SendStickerAsync(message.Chat.Id, stickers.Stickers.Where(s => s.Emoji == "😃").First().FileId);
                            bot.SendTextMessageAsync(
                                message.Chat.Id,
                                $"Поздравляю, {firstName}! Ты один из счастливчиков, выигравших печенье от Эльвиры! Твой номер {number + 1} и твоя страшная фраза:\n \"{word}\"");

                        }
                    }
                    break;
                case BotCommands.Movie:
                    var allMovies = Directory.GetDirectories(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "movies"));
                    var randomMovieDirectory = allMovies[new Random().Next(0, allMovies.Length)];
                    var posterPath = Path.Combine(randomMovieDirectory, "poster.png");
                    var description = File.ReadAllText(Path.Combine(randomMovieDirectory, "description"));
                    await using (var stream = File.OpenRead(posterPath))
                    {
                        await bot.SendPhotoAsync(message.Chat.Id, new InputOnlineFile(stream), description, ParseMode.Markdown, cancellationToken: cancellationToken);
                    }
                    break;
                default:
                    await bot.SendTextMessageAsync(message.Chat.Id, "Я не понимаю тебя, попробуй еще раз", cancellationToken: cancellationToken);
                    break;
            }
        }
    }
}

async Task HandleError(ITelegramBotClient bot, Exception exception, CancellationToken cancellationToken)
{
    Console.WriteLine(exception);
    Environment.Exit(1);
}

class WinnersWithWord
{
    public long TelegramId { get; }
    public int Number { get; set; }
    public string Nickname { get; set; }
    public string FirstName { get; set; }
    public string Word { get; set; }

    public WinnersWithWord(long telegramId, int number, string firstName, string nickname, string word)
    {
        this.TelegramId = telegramId;
        this.Number = number;
        this.FirstName = firstName;
        this.Nickname = nickname;
        this.Word = word;
    }
}
