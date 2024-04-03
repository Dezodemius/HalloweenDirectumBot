﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BotCommon;
using BotCommon.Repository;
using BotCommon.Scenarios;
using DirectumCareerNightBot.Scenarios;
using Newtonsoft.Json;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

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

        _activeUsersManager.Add(new BotUser(userId));

        UserCommandScenario userScenario = null;
        switch (GetMessage(update))
        {
            case BotChatCommands.Start:
            {
                var replyMarkup = MainMenuCommand.GetMainMenuInlineMarkup();
                await botClient.SendTextMessageAsync(userId, 
                    BotMessages.BotStartMessage, 
                    replyMarkup: replyMarkup, 
                    cancellationToken: cancellationToken);
                _userScenarioRepository.Remove(userId);
                return;
            }
            case BotChatCommands.MainMenu:
            {
                var replyMarkup = MainMenuCommand.GetMainMenuInlineMarkup();
                if (update.CallbackQuery?.Message != null)
                    await botClient.EditMessageTextAsync(userId,
                        update.CallbackQuery.Message.MessageId,
                        BotMessages.MainMenu,
                        replyMarkup: replyMarkup,
                        cancellationToken: cancellationToken);
                else
                {
                    await botClient.SendTextMessageAsync(userId,
                        BotMessages.MainMenu,
                        replyMarkup: replyMarkup,
                        cancellationToken: cancellationToken);
                }
                _userScenarioRepository.Remove(userId);
                return;
            }
            case BotChatCommands.WantToPractice:
                userScenario = new UserCommandScenario(userId, new PracticeScenario());
                break;
            case BotChatCommands.NotStudentButWantInIT:
                userScenario = new UserCommandScenario(userId, new NotStudentButWantInITScenario());
                break;
            case BotChatCommands.WorkInITButWantToChangeCompany:
                userScenario = new UserCommandScenario(userId, new WorkInITButWantToChangeCompanyScenario());
                break;
            case BotChatCommands.StudentWithExperience:
                userScenario = new UserCommandScenario(userId, new StudentWithExperienceScenario());
                break;
            case BotChatCommands.WorkerInITDept:
                userScenario = new UserCommandScenario(userId, new WorkingITDeptScenario());
                break;
            case BotChatCommands.Directum15Questions:
                var buttons = new List<InlineKeyboardButton[]>
                {
                    new [] {InlineKeyboardButton.WithCallbackData(BotMessages.MainMenuButton, BotChatCommands.MainMenu), }
                };
                var markup = new InlineKeyboardMarkup(buttons);
                await botClient.SendTextMessageAsync(
                    userId, 
                    BotMessages.Directum15QuestionsMessage, 
                    ParseMode.Markdown, 
                    replyMarkup: markup, 
                    cancellationToken: cancellationToken);
                break;
            case BotChatCommands.RafflePrizes:
                break;
        }
        if (userScenario == null && _userScenarioRepository.TryGet(userId, out var _userScenario))
            userScenario = _userScenario;
        else
            _userScenarioRepository.AddOrReplace(userScenario);

        if (userScenario != null && !(await userScenario.Run(botClient, update, userId)))
            _userScenarioRepository.Remove(userScenario);
    }

    public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        log.Error(exception);
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