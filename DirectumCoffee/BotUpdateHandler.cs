﻿using System.Text;
using BotCommon;
using BotCommon.Repository;
using BotCommon.Scenarios;
using Newtonsoft.Json;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace DirectumCoffee;

public class BotUpdateHandler : IUpdateHandler
{
    private static readonly ILogger log = LogManager.GetCurrentClassLogger();

    private static UserScenarioRepository? _userScenarioRepository;

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        log.Trace(JsonConvert.SerializeObject(update));
        var userInfo = BotHelper.GetUserInfo(update);
        log.Info($"user: {BotHelper.GetUsername(userInfo)}, userMessage: {BotHelper.GetMessage(update)}");
        var userId = userInfo.Id;
        BotDbContext.Instance.Add(new BotUser(
            userId, 
            userInfo.Username,
            userInfo.FirstName,
            userInfo.LastName,
            userInfo.LanguageCode));
        FillUserSystemInfo(userId);
        UserCommandScenario? userScenario = null;
        
        switch (BotHelper.GetMessage(update))
        {
            case BotChatCommands.Start:
            {
                var isFirstMeet = !BotDbContext.Instance.UserInfos.Any(u => u.UserId == userId);
                if (isFirstMeet)
                {
                    var replyMarkup = new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData(BotMessages.GoMessage, BotChatCommands.Go));
                    await botClient.SendTextMessageAsync(userId,
                        BotMessages.BotFirstMeet,
                        cancellationToken: cancellationToken,
                        parseMode: ParseMode.MarkdownV2,
                        replyMarkup: replyMarkup);
                }
                else
                {
                    var userSystemInfo = BotDbContext.Instance.UserSystemInfos
                        .Where(i => i.UserId == userId)
                        .FirstOrDefault();
                    InlineKeyboardMarkup replyMarkup;
                    if (userSystemInfo.SearchDisable)
                    {
                        replyMarkup = new InlineKeyboardMarkup(new []
                        {
                            new []{InlineKeyboardButton.WithCallbackData(BotMessages.MyInfo, BotChatCommands.Info)},
                            new []{InlineKeyboardButton.WithCallbackData(BotMessages.RestartInfo, BotChatCommands.Restart)},
                        });
                    }
                    else
                    {
                        replyMarkup = new InlineKeyboardMarkup(new []
                        {
                            new []{InlineKeyboardButton.WithCallbackData(BotMessages.MyInfo, BotChatCommands.Info)},
                            new []{InlineKeyboardButton.WithCallbackData(BotMessages.StopInfo, BotChatCommands.Stop)},
                        });
                    }
                    await botClient.SendTextMessageAsync(userId,
                        BotMessages.BotStartMessage,
                        cancellationToken: cancellationToken,
                        parseMode: ParseMode.MarkdownV2,
                        replyMarkup: replyMarkup);
                }
                _userScenarioRepository?.Remove(userId);
                break;
            }
            case BotChatCommands.Stop:
            {
                var userSystemInfo = BotDbContext.Instance.UserSystemInfos
                    .Where(i => i.UserId == userId)
                    .FirstOrDefault();
                userSystemInfo.SearchDisable = true;
                await BotDbContext.Instance.SaveChangesAsync(cancellationToken);
                await botClient.SendTextMessageAsync(userId, BotMessages.StopInfoMessage, cancellationToken: cancellationToken, parseMode: ParseMode.MarkdownV2);
                
                _userScenarioRepository?.Remove(userId);
                break;
            }
            case BotChatCommands.Restart:
            {
                var userSystemInfo = BotDbContext.Instance.UserSystemInfos
                    .Where(i => i.UserId == userId)
                    .FirstOrDefault();
                userSystemInfo.SearchDisable = false;
                await BotDbContext.Instance.SaveChangesAsync(cancellationToken);
                
                await botClient.SendTextMessageAsync(userId, BotMessages.RestartInfoMessage, cancellationToken: cancellationToken, parseMode: ParseMode.MarkdownV2);
                
                _userScenarioRepository?.Remove(userId);
                break;
            }
            case BotChatCommands.Go:
            {
                userScenario = new UserCommandScenario(userId, new MainScenario());
                break;
            }
            case BotChatCommands.Info:
            {
                var info = BotDbContext.Instance.UserInfos
                    .Where(u => u.UserId == userId)
                    .FirstOrDefault();
                var userInfoText = new StringBuilder();
                if (info == null)
                {
                    userInfoText.Append(BotMessages.InfoNotFound);
                }
                else
                {
                    userInfoText.AppendLine($"Имя: {info.Name}");
                    userInfoText.AppendLine($"Город: {info.City}");
                    userInfoText.AppendLine($"Направление: {info.Work}");
                    userInfoText.AppendLine($"Увлечения: {info.Hobby}");
                    userInfoText.AppendLine($"О чём хочешь пообщаться: {info.Interests}");
                }

                var replyMarkup = new InlineKeyboardMarkup(new []
                {
                    new []{InlineKeyboardButton.WithCallbackData("Заполнить заново", BotChatCommands.Go)},
                    new []{InlineKeyboardButton.WithCallbackData(BotMessages.ChangeInfo, BotChatCommands.Change)},
                    new []{InlineKeyboardButton.WithCallbackData(BotMessages.BackButton, BotChatCommands.Start)},
                });
                
                await botClient.SendTextMessageAsync(userId, userInfoText.ToString(), cancellationToken: cancellationToken, replyMarkup: replyMarkup);
                _userScenarioRepository?.Remove(userId);
                break;
            }
            case BotChatCommands.Change:
            {
                var replyMarkup = new InlineKeyboardMarkup(new []
                {
                    new []{InlineKeyboardButton.WithCallbackData("Имя", BotChatCommands.ChangeName)},
                    new []{InlineKeyboardButton.WithCallbackData("Город", BotChatCommands.ChangeCity)},
                    new []{InlineKeyboardButton.WithCallbackData("Направление", BotChatCommands.ChangeWork)},
                    new []{InlineKeyboardButton.WithCallbackData("Увлечения", BotChatCommands.ChangeHobby)},
                    new []{InlineKeyboardButton.WithCallbackData("О чём хочешь пообщаться", BotChatCommands.ChangeInterests)},
                    new []{InlineKeyboardButton.WithCallbackData(BotMessages.BackButton, BotChatCommands.Start)}
                });
                await botClient.SendTextMessageAsync(userId, "Выбери, что хочешь изменить", replyMarkup: replyMarkup);
                _userScenarioRepository?.Remove(userId);
                break;
            }
            case BotChatCommands.ChangeName:
            {
                userScenario = new UserCommandScenario(userId, new ChangeNameScenario());
                break;
            }
            case BotChatCommands.ChangeCity:
            {
                userScenario = new UserCommandScenario(userId, new ChangeCityScenario());
                break;
            }
            case BotChatCommands.ChangeWork:
            {
                userScenario = new UserCommandScenario(userId, new ChangeWorkScenario());
                break;
            }
            case BotChatCommands.ChangeHobby:
            {
                userScenario = new UserCommandScenario(userId, new ChangeHobbyScenario());
                break;
            }
            case BotChatCommands.ChangeInterests:
            {
                userScenario = new UserCommandScenario(userId, new ChangeInterestsScenario());
                break;
            }
        }
        if (userScenario == null && _userScenarioRepository.TryGet(userId, out var _userScenario))
            userScenario = _userScenario;
        else
            _userScenarioRepository.AddOrReplace(userScenario);

        if (userScenario != null && !(await userScenario.Run(botClient, update, userId)))
            _userScenarioRepository.Remove(userScenario);
    }

    private static void FillUserSystemInfo(long userId)
    {
        var info = BotDbContext.Instance.UserSystemInfos
            .Where(i => i.UserId == userId)
            .FirstOrDefault();
        if (info == null)
        {
            var userInfo = new UserSystemInfo();
            userInfo.UserId = userId;
            userInfo.PairFound = false;
            userInfo.SearchDisable = false;
            BotDbContext.Instance.UserSystemInfos.Add(userInfo);
            BotDbContext.Instance.SaveChanges();
        }
    }

    public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        log.Error(exception);
        Environment.Exit(0);
    }

    public BotUpdateHandler()
    {
        _userScenarioRepository = new UserScenarioRepository();
    }
}