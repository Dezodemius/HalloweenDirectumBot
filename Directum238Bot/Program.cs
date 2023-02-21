using System;
using System.IO;
using System.Threading.Tasks;
using BotCommon;
using BotCommon.KeepAlive;
using BotCommon.Repository;
using Newtonsoft.Json;
using NLog;
using Telegram.Bot;
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

    public static void Main(string[] args)
    {
      var bot = new TelegramBotClient(new BotConfigManager().Config.BotToken);
      PrepareForStartBot(bot);
      // StartAnimationScheduleMessage(bot, Schedule.Day23AnonsMessageDateTime, Directum238BotResources.AnonsWish23, "4.gif");
      // StartTextScheduleMessage(bot, Schedule.LasertagMessageDateTime, Directum238BotResources.LaserTagMessage);
      // StartAnimationScheduleMessage(bot, Schedule.Day8AnonsMessageDateTime, Directum238BotResources.AnonsWish8, "5.gif");
      // StartTextScheduleMessage(bot, Schedule.MorningMeetingMessageDateTime, Directum238BotResources.MorningMeetingWomenDay);
      StartBot(bot);
      string command;
      do
      {
        command = Console.ReadLine();
      } while (!command.Equals("/exit", StringComparison.InvariantCulture));
      log.Info("Bye bye");
      Environment.Exit(0);
    }

    private static void StartAnimationScheduleMessage(ITelegramBotClient bot, DateTime scheduleDate, string message, string gifName)
    {
      var timer = new System.Timers.Timer(TimeSpan.FromSeconds(5));
      timer.AutoReset = true;
      timer.Elapsed += (a, b) =>
      {
        if (DateTime.Now.CompareTo(scheduleDate) < 0)
          return;

        var usersId = _activeUsersManager.GetAll();
        var gif = new InputOnlineFile(File.OpenRead(GetGifPath(gifName)), gifName);
        var markup =
            new InlineKeyboardMarkup(
              InlineKeyboardButton.WithCallbackData(Directum238BotResources.GoStartMenu, BotChatCommand.MainMenu));
        foreach (var user in usersId)
        {
          bot.SendAnimationAsync(user.BotUserId,
            animation: gif,
            caption: message,
            replyMarkup: markup,
            parseMode: ParseMode.Markdown);
        }
        if (a is System.Timers.Timer timer)
          timer.Dispose();
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
        LogManager.GetCurrentClassLogger().Debug(args);
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
}

