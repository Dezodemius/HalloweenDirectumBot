using System;
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

    private static UserScenarioRepository? _userScenarioRepository;

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        log.Info(JsonConvert.SerializeObject(update));
        var userInfo = BotHelper.GetUserInfo(update);
        var userId = userInfo.Id;

        await using var quizContext = new BotDbContext();
        quizContext?.Add(new BotUser(
            userId, 
            userInfo.Username,
            userInfo.FirstName,
            userInfo.LastName,
            userInfo.LanguageCode));

        UserCommandScenario? userScenario = null;
        switch (BotHelper.GetMessage(update))
        {
            case BotChatCommands.Start:
            {
                var replyMarkup = MainMenuCommand.GetMainMenuInlineMarkup();
                await botClient.SendTextMessageAsync(userId, 
                    BotMessages.BotStartMessage, 
                    replyMarkup: replyMarkup, 
                    cancellationToken: cancellationToken);
                _userScenarioRepository?.Remove(userId);
                return;
            }
            case BotChatCommands.MainMenu:
            {
                var replyMarkup = MainMenuCommand.GetMainMenuInlineMarkup();
                await botClient.SendTextMessageAsync(userId,
                    BotMessages.MainMenu,
                    replyMarkup: replyMarkup,
                    cancellationToken: cancellationToken);
                _userScenarioRepository?.Remove(userId);
                return;
            }
            case BotChatCommands.Student:
            {
                var buttons = new List<InlineKeyboardButton[]>
                {
                    new [] { InlineKeyboardButton.WithCallbackData(BotMessages.WantToPractice, BotChatCommands.WantToPractice), },
                    new [] { InlineKeyboardButton.WithCallbackData(BotMessages.StudentWithExperience, BotChatCommands.StudentWithExperience), },
                    new [] { InlineKeyboardButton.WithCallbackData(BotMessages.Directum15Questions, BotChatCommands.Directum15Questions), },
                    new [] { InlineKeyboardButton.WithCallbackData(BotMessages.MainMenuButton, BotChatCommands.MainMenu), },
                };

                var replyMarkup = new InlineKeyboardMarkup(buttons);
                await botClient.SendTextMessageAsync(userId,
                    BotMessages.StudentMessage,
                    replyMarkup: replyMarkup,
                    cancellationToken: cancellationToken);
                _userScenarioRepository?.Remove(userId);
                break;
            }
            case BotChatCommands.NotStudent:
            {
                var buttons = new List<InlineKeyboardButton[]>
                {
                    new [] { InlineKeyboardButton.WithCallbackData(BotMessages.NotStudentButWantInIT, BotChatCommands.NotStudentButWantInIT), },
                    new [] { InlineKeyboardButton.WithCallbackData(BotMessages.WorkerInITDept, BotChatCommands.WorkerInITDept), },
                    new [] { InlineKeyboardButton.WithCallbackData(BotMessages.MainMenuButton, BotChatCommands.MainMenu), },
                };

                var replyMarkup = new InlineKeyboardMarkup(buttons);
                await botClient.SendTextMessageAsync(userId,
                    BotMessages.NotStudentMessage,
                    replyMarkup: replyMarkup,
                    cancellationToken: cancellationToken);
                _userScenarioRepository?.Remove(userId);
                break;
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
            {
                var buttons = new List<InlineKeyboardButton[]>
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(BotMessages.MainMenuButton, BotChatCommands.MainMenu),
                    }
                };
                var markup = new InlineKeyboardMarkup(buttons);
                await botClient.SendTextMessageAsync(
                    userId,
                    BotMessages.Directum15QuestionsMessage,
                    ParseMode.Markdown,
                    replyMarkup: markup,
                    cancellationToken: cancellationToken);
                break;
            }
            case BotChatCommands.RafflePrizes:
                userScenario = new UserCommandScenario(userId, new QuizScenario());
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

    public BotUpdateHandler()
    {
        _userScenarioRepository = new UserScenarioRepository();
    }
}