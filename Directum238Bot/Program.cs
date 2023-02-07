using BotCommon;
using BotCommon.Scenarios;
using Directum238Bot.Scenarios;
using Newtonsoft.Json;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Directum238Bot
{
  public static class Program
  {
    private static readonly ILogger log = LogManager.GetCurrentClassLogger();

    private static UserScenarioRepository _userScenarioRepository;
    private static BotConfigManager _configManager;
    public static void Main(string[] args)
    {
      PrepareForStartBot();
      StartBot();
      string command;
      do
      {
        command = Console.ReadLine();
      } while (!command.Equals("/exit", StringComparison.InvariantCulture));
      Environment.Exit(0);
    }

    private static void PrepareForStartBot()
    {
      _configManager = new BotConfigManager();
      _userScenarioRepository = new UserScenarioRepository(_configManager.Config.DbConnectionString);

      ChatScenarioCache.Register(new StartScenario());
    }

    private static void StartBot()
    {
      log.Debug("Start Bot");
      var bot = new TelegramBotClient(new BotConfigManager().Config.BotToken);
      var opts = new ReceiverOptions()
      {
        AllowedUpdates = new []
        {
          UpdateType.Message,
          UpdateType.CallbackQuery,
          UpdateType.EditedMessage,
        }
      };
      bot.StartReceiving(UpdateHandler, PollingErrorHandler, receiverOptions: opts);
    }

    private static async Task PollingErrorHandler(ITelegramBotClient bot, Exception exception, CancellationToken ct)
    {
      LogManager.GetCurrentClassLogger().Debug(exception);
    }

    private static async Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
      log.Debug(JsonConvert.SerializeObject(update));
      var userId = update.Message.From.Id;
      var userScenario = _userScenarioRepository.Get(userId);

      if (userScenario == null)
      {
        userScenario = new UserScenario(userId, ChatScenarioCache.FindByCommandName(BotChatCommand.Start));
        _userScenarioRepository.Add(userScenario);
      }
      if (!userScenario.Run(bot, update))
        _userScenarioRepository.Remove(userScenario);
    }
  }
}

