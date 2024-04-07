using System;
using System.Threading.Tasks;
using BotCommon;
using BotCommon.KeepAlive;
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

    public static void Main()
    {
      try
      {
        var bot = new TelegramBotClient(new BotConfigManager().Config.BotToken);
        PrepareForStartBot(bot);
        StartBot(bot);
      }
      catch (Exception e)
      {
        log.Fatal(e);
        throw;
      }
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
      var botKeepAlive = new BotKeepAlive(bot);
      botKeepAlive.StartKeepAlive();
    }

    private static void StartBot(ITelegramBotClient bot)
    {
      log.Debug("Start Bot");
      var opts = new ReceiverOptions
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

