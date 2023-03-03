using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using BotCommon;
using BotCommon.KeepAlive;
using BotCommon.Repository;
using Newtonsoft.Json;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using File = System.IO.File;

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

    private static void StartAnimationScheduleMessage(ITelegramBotClient bot, string fileName, DateTime scheduleDate,
      string message, string gifName)
    {
      bool needToNotificate;
      var scheduledMessageInfoFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
      if (!File.Exists(scheduledMessageInfoFilePath))
      {
        needToNotificate = true;
      }
      else
      {
        var scheduledMessageInfo = JsonConvert.DeserializeObject<ScheduledMessageInfo>(File.ReadAllText(scheduledMessageInfoFilePath));
        needToNotificate = scheduledMessageInfo.NeedToNotificate;
        scheduleDate = scheduledMessageInfo.MessageDateTime;
      }

      if (!needToNotificate)
        return;

      var timer = new System.Timers.Timer(TimeSpan.FromSeconds(5));
      timer.AutoReset = true;
      timer.Elapsed += (sender, b) =>
      {
        if (DateTime.Now.CompareTo(scheduleDate) < 0)
          return;
        System.Timers.Timer timer = null;
        if (sender is System.Timers.Timer)
          timer = (System.Timers.Timer)sender;
        timer.Enabled = false;
        var scheduleMessageInfo = new ScheduledMessageInfo { NeedToNotificate = false, };
        File.WriteAllText(scheduledMessageInfoFilePath, JsonConvert.SerializeObject(scheduleMessageInfo));
        var usersId = _activeUsersManager.GetAll().GetEnumerator();
        var markup = new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData(Directum238BotResources.GoStartMenu, BotChatCommand.MainMenu));

        var hasUsersToBroadcast = usersId.MoveNext();
        while (hasUsersToBroadcast)
        {
          var user = usersId.Current;
          var userInfo = bot.GetChatMemberAsync(user.BotUserId, user.BotUserId).Result;
          try
          {
            log.Info($"Sending message to: {userInfo.User.FirstName} {userInfo.User}");
            _ = bot.SendAnimationAsync(user.BotUserId,
              animation: new InputOnlineFile(File.OpenRead(GetGifPath(gifName)), gifName)).Result;
            _ = bot.SendTextMessageAsync(user.BotUserId,
              text: message,
              replyMarkup: markup,
              parseMode: ParseMode.Markdown).Result;
            Thread.Sleep(3000);
          }
          catch (AggregateException e)
              when(e.InnerException is ApiRequestException apiRequestException
                   && apiRequestException.ErrorCode == (int)HttpStatusCode.TooManyRequests)
          {
            log.Warn($"Failed to send message to {user.BotUserId}", e);
            Thread.Sleep(TimeSpan.FromSeconds((double)apiRequestException.Parameters.RetryAfter));
            continue;
          }

          log.Info($"Message sent to: {userInfo.User.FirstName} {userInfo.User}");
          hasUsersToBroadcast = usersId.MoveNext();
        }
      };
      timer.Start();
    }

    private static void StartTextScheduleMessage(ITelegramBotClient bot, DateTime scheduleDate, string message)
    {
      var timer = new System.Timers.Timer(TimeSpan.FromSeconds(5));
      timer.AutoReset = true;
      timer.Elapsed += (a, b) =>
      {
        if (DateTime.Now.CompareTo(scheduleDate) < 0)
          return;

        var usersId = _activeUsersManager.GetAll();
        var markup =
            new InlineKeyboardMarkup(
              InlineKeyboardButton.WithCallbackData(Directum238BotResources.GoStartMenu, BotChatCommand.MainMenu));
        foreach (var user in usersId)
        {
          bot.SendTextMessageAsync(user.BotUserId,
            text: message,
            replyMarkup: markup,
            parseMode: ParseMode.Markdown);
        }
        if (a is System.Timers.Timer timer)
          timer.Dispose();
      };
      timer.Start();
    }

    private static string GetGifPath(string gifFileName)
    {
      return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GIFs", gifFileName);
    }

    private static void PrepareForStartBot(TelegramBotClient bot)
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
      bot.OnApiResponseReceived += (client, args, token) =>
      {
        LogManager.GetCurrentClassLogger().Debug(JsonConvert.SerializeObject(args));
        return ValueTask.CompletedTask;
      };
      bot.OnApiResponseReceived += (client, args, token) =>
      {
        LogManager.GetCurrentClassLogger().Debug(JsonConvert.SerializeObject(args));
        return ValueTask.CompletedTask;
      };
      bot.OnMakingApiRequest += (client, args, token) =>
      {
        LogManager.GetCurrentClassLogger().Debug(JsonConvert.SerializeObject(args));
        return ValueTask.CompletedTask;
      };
      // bot.StartReceiving(UpdateHandler, PollingErrorHandler, receiverOptions: opts);
      bot.StartReceiving<BotUpdateHandler>(receiverOptions: opts);
    }
  }

  public class ScheduledMessageInfo
  {
    public DateTime MessageDateTime { get; set; }
    public bool NeedToNotificate { get; set; }
    public string MessageName { get; set; }
  }
}

