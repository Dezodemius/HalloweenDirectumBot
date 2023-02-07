using BotCommon;
using Directum238Bot.Scenarios;
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

    private static ScenarioManager _scenarioManager;
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
      _scenarioManager = new ScenarioManager(_configManager.Config.DbConnectionString);
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

    private static Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
      var userId = update.Message.From.Id;
      var userScenario = _scenarioManager.UserScenarios.SingleOrDefault(s => s.UserId == userId);
      if (userScenario != null)
      {
        if (!userScenario.Run(bot, update))
          _scenarioManager.UserScenarios.Remove(userScenario);
      }
      else
      {
        _scenarioManager.UserScenarios
          .Add(new UserScenario(userId, new StartScenario()));
      }
      // switch (update.Type)
      // {
      //   case UpdateType.Message:
      //   {
      //     
      //     // var message = update.Message;
      //     // if (message is { Text: { } } )
      //     // {
      //     //   if (UpdateMessageIsCommand(message.Text))
      //     //   {
      //     //     ProcessCommandUpdate(message);
      //     //   }
      //     // }
      //     break;
      //   }
      //   case UpdateType.CallbackQuery:
      //   {
      //     break;
      //   }
      //   case UpdateType.EditedMessage:
      //   {
      //     break;
      //   }
      //   default:
      //   {
      //     log.Info($"Unknown or unsupported update type: {update.Type}");
      //     break;
      //   }
      // }

      return Task.CompletedTask;
    }

    // private static async void ProcessCommandUpdate(Message message)
    // {
    //   var messageText = message.Text;
    //   var chatId = message.Chat.Id;
    //   var userId = message.From.Id;
    //   switch (messageText)
    //   {
    //     case BotChatCommand.Start:
    //     {
    //       var currentUserScenario = _scenarioManager.UserScenarios.Select(s => s.UserId == userId);
    //       currentUserScenario.
    //       // TODO: Приветствие.
    //       break;
    //     }
    //   }
    // }
    //
    // private static bool UpdateMessageIsCommand(string updateMessage)
    // {
    //   return !string.IsNullOrWhiteSpace(updateMessage) && BotChatCommands.Commands.Any(c => c == updateMessage);
    // }
  }
}

