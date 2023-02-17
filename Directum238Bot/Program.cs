using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BotCommon;
using BotCommon.KeepAlive;
using BotCommon.Repository;
using BotCommon.Scenarios;
using Directum238Bot.Repository;
using Directum238Bot.Scenarios;
using Newtonsoft.Json;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using File = System.IO.File;

namespace Directum238Bot
{
  public static class Program
  {
    private static readonly ILogger log = LogManager.GetCurrentClassLogger();

    private static UserScenarioRepository _userScenarioRepository;
    private static ActiveUsersManager _activeUsersManager;
    private static BotConfigManager _configManager;
    private static UserContentCache _contentCache;

    public static void Main(string[] args)
    {
      var bot = new TelegramBotClient(new BotConfigManager().Config.BotToken);
      PrepareForStartBot(bot);
      StartAnimationScheduleMessage(bot, Schedule.Day23AnonsMessageDateTime, Directum238BotResources.AnonsWish23, "4.gif");
      StartTextScheduleMessage(bot, Schedule.LasertagMessageDateTime, Directum238BotResources.LaserTagMessage);
      StartAnimationScheduleMessage(bot, Schedule.Day8AnonsMessageDateTime, Directum238BotResources.AnonsWish8, "5.gif");
      StartTextScheduleMessage(bot, Schedule.MorningMeetingMessageDateTime, Directum238BotResources.MorningMeetingWomenDay);
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
      _userScenarioRepository = new UserScenarioRepository();
      _contentCache = new UserContentCache(_configManager.Config.DbConnectionString);
      _activeUsersManager = new ActiveUsersManager(_configManager.Config.DbConnectionString);
      var botKeepAlive = new BotKeepAlive(bot);
      botKeepAlive.StartKeepAlive();
    }

    private static void StartBot(TelegramBotClient bot)
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
      if (userId != default)
        return;
      _activeUsersManager.Add(new BotUser(userId));

      UserCommandScenario userScenario = null;
      switch (GetMessage(update))
      {
        case BotChatCommand.Start:
        {
          userScenario = new UserCommandScenario(userId, new StartScenario());
          break;
        }
        case BotChatCommand.SendWish23:
        {
          userScenario = new UserCommandScenario(userId, new SendWishScenario(_contentCache, WishDay.Day23));
          break;
        }
        case BotChatCommand.SendWish8:
        {
          userScenario = new UserCommandScenario(userId, new SendWishScenario(_contentCache, WishDay.Day8));
          break;
        }
        case BotChatCommand.GetWish23:
        {
          userScenario = new UserCommandScenario(userId, new GetWishScenario(_contentCache, WishDay.Day23));
          break;
        }
        case BotChatCommand.GetWish8:
        {
          userScenario = new UserCommandScenario(userId, new GetWishScenario(_contentCache, WishDay.Day8));
          break;
        }
        case BotChatCommand.Broadcast:
        {
          if (_configManager.Config.BotAdminId.Contains(userId))
            userScenario = new UserCommandScenario(userId, new BroadcastScenario(_activeUsersManager));
          break;
        }
        case BotChatCommand.MainMenu:
        {
          var inlineButtons = new List<InlineKeyboardButton[]>
          {
              new [] { InlineKeyboardButton.WithCallbackData(Directum238BotResources.SendWish23, BotChatCommand.SendWish23) },
              new [] { InlineKeyboardButton.WithCallbackData(Directum238BotResources.SendWish8, BotChatCommand.SendWish8) }
          };
          if (DateTime.Now.CompareTo(Schedule.Day23AnonsMessageDateTime) >= 0)
            inlineButtons.Add(new [] {InlineKeyboardButton.WithCallbackData(Directum238BotResources.GetWish23, BotChatCommand.GetWish23)});
          if (DateTime.Now.CompareTo(Schedule.Day8AnonsMessageDateTime) >= 0)
            inlineButtons.Add(new [] { InlineKeyboardButton.WithCallbackData(Directum238BotResources.GetWish8, BotChatCommand.GetWish8) });
          var markup = new InlineKeyboardMarkup(inlineButtons);
          await bot.SendTextMessageAsync(userId,
            Directum238BotResources.MainMenuText,
            replyMarkup: markup,
            parseMode: ParseMode.Markdown, cancellationToken: ct);
          break;
        }
      }

      if (userScenario == null && _userScenarioRepository.TryGet(userId, out var _userScenario))
        userScenario = _userScenario;
      else
        _userScenarioRepository.AddOrReplace(userScenario);

      if (userScenario != null && !userScenario.Run(bot, update, userId))
        _userScenarioRepository.Remove(userScenario);
    }

    private static string GetMessage(Update update)
    {
      switch (update.Type)
      {
        case UpdateType.Message:
          return update.Message.Text;
        case UpdateType.CallbackQuery:
          return update.CallbackQuery.Data;
        default:
          return null;
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
        default:
          return default;
      }
    }
  }
}

