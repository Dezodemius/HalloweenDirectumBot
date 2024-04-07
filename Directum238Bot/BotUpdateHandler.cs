using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BotCommon;
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
using Telegram.Bot.Types.ReplyMarkups;

namespace Directum238Bot;

public class BotUpdateHandler : IUpdateHandler
{
  private static readonly ILogger log = LogManager.GetCurrentClassLogger();
  private static UserScenarioRepository _userScenarioRepository;
  private static UserDbContext _userDbContext;
  private static BotConfigManager _configManager;
  private static UserContentCache _contentCache;

  public async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
  {
    log.Info(JsonConvert.SerializeObject(update));
    var userId = GetUserId(update);
    if (userId == default)
      return;

    _userDbContext.Add(new BotUser(userId));

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
          userScenario = new UserCommandScenario(userId, new BroadcastScenario(_userDbContext));
        break;
      }
      case BotChatCommand.MainMenu:
      {
        var inlineButtons = new List<InlineKeyboardButton[]>
        {
          // new[] { InlineKeyboardButton.WithCallbackData(Directum238BotResources.SendWish23, BotChatCommand.SendWish23) },
          new[] { InlineKeyboardButton.WithCallbackData(Directum238BotResources.SendWish8, BotChatCommand.SendWish8) }
        };
        // if (DateTime.Now.CompareTo(Schedule.Day23AnonsMessageDateTime) >= 0)
          // inlineButtons.Add(new[] { InlineKeyboardButton.WithCallbackData(Directum238BotResources.GetWish23, BotChatCommand.GetWish23) });
        if (DateTime.Now.CompareTo(Schedule.Day8AnonsMessageDateTime) >= 0)
          inlineButtons.Add(new[] { InlineKeyboardButton.WithCallbackData(Directum238BotResources.GetWish8, BotChatCommand.GetWish8) });
        var markup = new InlineKeyboardMarkup(inlineButtons);
        if (update.CallbackQuery.Message.Type != MessageType.Text)
        {
          await bot.DeleteMessageAsync(userId,
            update.CallbackQuery.Message.MessageId,
            cancellationToken: cancellationToken);
          await bot.SendTextMessageAsync(userId,
            Directum238BotResources.MainMenuText,
            replyMarkup: markup,
            parseMode: ParseMode.Markdown,
            cancellationToken: cancellationToken);
        }
        await bot.EditMessageTextAsync(userId,
          update.CallbackQuery.Message.MessageId,
          Directum238BotResources.MainMenuText,
          replyMarkup: markup,
          parseMode: ParseMode.Markdown,
          cancellationToken: cancellationToken);
        break;
      }
    }

    if (userScenario == null && _userScenarioRepository.TryGet(userId, out var _userScenario))
      userScenario = _userScenario;
    else
      _userScenarioRepository.AddOrReplace(userScenario);

    if (userScenario != null && !(await userScenario.Run(bot, update, userId)))
      _userScenarioRepository.Remove(userScenario);
  }

  private static long GetUserId(Update update)
  {
    return update.Type switch
    {
        UpdateType.Message => update.Message.From.Id,
        UpdateType.CallbackQuery => update.CallbackQuery.From.Id,
        _ => default
    };
  }

  public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
  {
    LogManager.GetCurrentClassLogger().Error(exception);
    try
    {
      foreach (var botAdmin in _configManager.Config.BotAdminId)
        await botClient.SendTextMessageAsync(botAdmin, "Бот упал", cancellationToken: cancellationToken);
    }
    catch (Exception e)
    {
      log.Error(e);
    }
    finally
    {
      Environment.Exit(1);
    }
  }

  public BotUpdateHandler()
  {
    _configManager = new BotConfigManager();
    _userScenarioRepository = new UserScenarioRepository();
    _contentCache = new UserContentCache(_configManager.Config.DbConnectionString);
    _userDbContext = new UserDbContext(_configManager.Config.DbConnectionString);
  }
}