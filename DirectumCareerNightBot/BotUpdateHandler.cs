using System;
using System.Threading;
using System.Threading.Tasks;
using BotCommon;
using BotCommon.Repository;
using BotCommon.Scenarios;
using Newtonsoft.Json;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DirectumCareerNightBot;

public class BotUpdateHandler : IUpdateHandler
{
    private static readonly ILogger log = LogManager.GetCurrentClassLogger();

    private static BotConfigManager _configManager;
    private static ActiveUsersManager _activeUsersManager;
    private static UserScenarioRepository _userScenarioRepository;

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        log.Info(JsonConvert.SerializeObject(update));
        var userId = GetUserId(update);
        if (userId == default)
            return;
        switch (GetMessage(update))
        {
            case BotChatCommands.Start:
                await botClient.SendTextMessageAsync(userId, BotMessages.BotStartMessage, cancellationToken: cancellationToken);
                break;
            case BotChatCommands.MainMenu:
                break;
            case BotChatCommands.Practice:
                break;
            case BotChatCommands.Alumnus:
                break;
            case BotChatCommands.Worker:
                break;
            case BotChatCommands.Student:
                break;
            case BotChatCommands.ITDeptWorker:
                break;
            case BotChatCommands.Q15:
                break;
            case BotChatCommands.Raffle:
                break;
            case BotChatCommands.Career:
                break;
            case BotChatCommands.Socials:
                break;
            
        }
        _activeUsersManager.Add(new BotUser(userId));
    }

    public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        try
        {
            // foreach (var botAdmin in _configManager.Config.BotAdminId)
            //     await botClient.SendTextMessageAsync(botAdmin, "Бот упал", cancellationToken: cancellationToken);
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

    private static string GetMessage(Update update)
    {
        return update.Type switch
        {
            UpdateType.Message => update.Message.Text,
            UpdateType.CallbackQuery => update.CallbackQuery.Data,
            _ => null
        };
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

    public BotUpdateHandler()
    {
        _configManager = new BotConfigManager();
        _activeUsersManager = new ActiveUsersManager(_configManager.Config.DbConnectionString);
        _userScenarioRepository = new UserScenarioRepository();
    }
}