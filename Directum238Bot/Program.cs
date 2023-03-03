using System;
using System.Threading.Tasks;
using BotCommon;
using BotCommon.KeepAlive;
using BotCommon.Repository;
using Newtonsoft.Json;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace Directum238Bot
{
  public static class Program
  {
    private static readonly ILogger log = LogManager.GetCurrentClassLogger();

    private static ActiveUsersManager _activeUsersManager;
    private static BotConfigManager _configManager;

    public static void Main()
    {
      var bot = new TelegramBotClient(new BotConfigManager().Config.BotToken);
      PrepareForStartBot(bot);
      StartBot(bot);
      string command;
      do
      {
        command = Console.ReadLine();
      } while (!command.Equals("/exit", StringComparison.InvariantCulture));
      log.Info("Bye bye");
      Environment.Exit(0);
    }

    private static void PrepareForStartBot(ITelegramBotClient bot)
    {
      _configManager = new BotConfigManager();
      _activeUsersManager = new ActiveUsersManager(_configManager.Config.DbConnectionString);
      var botKeepAlive = new BotKeepAlive(bot);
      botKeepAlive.StartKeepAlive();
    }

    private static void StartBot(ITelegramBotClient bot)
    {
      log.Debug("Start Bot");
      var opts = new ReceiverOptions()
      {
        AllowedUpdates = new []
        {
          UpdateType.Message,
          UpdateType.CallbackQuery
        },
        ThrowPendingUpdates = true
      };
      bot.OnApiResponseReceived += (_, args, _) =>
      {
        log.Debug($" <<<< {JsonConvert.SerializeObject(args)}");
        return ValueTask.CompletedTask;
      };
      bot.OnMakingApiRequest += (_, args, _) =>
      {
        log.Debug($" >>>> {JsonConvert.SerializeObject(args)}");
        return ValueTask.CompletedTask;
      };

      bot.StartReceiving<BotUpdateHandler>(receiverOptions: opts);
    }
  }
}

