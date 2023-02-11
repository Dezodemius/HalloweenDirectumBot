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
    private static ActiveUsersManager _activeUsersManager;
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
      _userScenarioRepository = new UserScenarioRepository();
      _activeUsersManager = new ActiveUsersManager(_configManager.Config.DbConnectionString);
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
        },
        ThrowPendingUpdates = true
      };
      bot.StartReceiving(UpdateHandler, PollingErrorHandler, receiverOptions: opts);
    }

    private static Task PollingErrorHandler(ITelegramBotClient bot, Exception exception, CancellationToken ct)
    {
      LogManager.GetCurrentClassLogger().Debug(exception);
      return bot.TestApiAsync(ct);
    }

    private static async Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
      log.Debug(JsonConvert.SerializeObject(update));
      var userId = GetUserId(update);
      if (userId == default)
        return;
      _activeUsersManager.AddUser(new BotUser(userId));
      // if (userId == _configManager.Config.BotAdminId)
      // {
      //   if (_userScenarioRepository.TryGet(userId, out var adminScenario))
      //     adminScenario.Run(bot, update);
      //   else
      //   {
      //     adminScenario = new UserCommandScenario(userId, new AdminMessageBroadcastScenario(_activeUsersManager));
      //     _userScenarioRepository.Add(adminScenario);
      //     adminScenario.Run(bot, update);
      //   }
      //   return;
      // }
      if (_userScenarioRepository.TryGet(userId, out var userScenario))
      {
        if (!userScenario.Run(bot, update))
        {
          _userScenarioRepository.Remove(userScenario);
          userScenario = null;
        };
      }
      if (userScenario == null)
      {
        if (update.Type != UpdateType.Message)
          return;

        switch (update.Message.Text)
        {
          case BotChatCommand.Start:
          {
            userScenario = new UserCommandScenario(userId, new StartScenario());
            break;
          }
          case BotChatCommand.Wish23:
          {
            userScenario = new UserCommandScenario(userId, new Wish23Scenario(new UserContentCache(_configManager.Config.DbConnectionString)));
            break;
          }
        }
        _userScenarioRepository.Add(userScenario);
        userScenario?.Run(bot, update);
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

