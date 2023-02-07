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
      log.Info("Bye bye");
      Environment.Exit(0);
    }

    private static void PrepareForStartBot()
    {
      _configManager = new BotConfigManager();
      _userScenarioRepository = new UserScenarioRepository(_configManager.Config.DbConnectionString);

      ChatScenarioCache.Register(new StartScenario());
      ChatScenarioCache.Register(new Wish23Scenario());
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

    private static Task PollingErrorHandler(ITelegramBotClient bot, Exception exception, CancellationToken ct)
    {
      LogManager.GetCurrentClassLogger().Debug(exception);
      return Task.CompletedTask;
    }

    private static async Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
      log.Debug(JsonConvert.SerializeObject(update));
      var userId = GetUserId(update);
      if (userId == default)
        return;

      if (_userScenarioRepository.TryGet(userId, out var userScenario))
      {
        if (!userScenario.Run(bot, update))
          _userScenarioRepository.Remove(userScenario);
      }

      if (update.Type == UpdateType.Message)
      {
        switch (update.Message.Text)
        {
          case BotChatCommand.Start:
          {
            userScenario = new UserScenario(userId, ChatScenarioCache.FindByCommandName(BotChatCommand.Start));
            break;
          }
          case BotChatCommand.Wish23:
          {
            userScenario = new UserScenario(userId, ChatScenarioCache.FindByCommandName(BotChatCommand.Wish23));
            break;
          }
        }
        _userScenarioRepository.Add(userScenario);
        userScenario.Run(bot, update);
      }
    }

    private static long GetUserId(Update update)
    {
      switch (update.Type)
      {
        case UpdateType.Message:
          return update.Message.From.Id;
        case UpdateType.CallbackQuery:
          return update.CallbackQuery.From.Id;
        case UpdateType.EditedMessage:
          return update.EditedMessage.From.Id;
        default:
          return default;
      }
    }
  }
}

