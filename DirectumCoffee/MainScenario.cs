﻿using BotCommon;
using BotCommon.Scenarios;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace DirectumCoffee;

public class MainScenario : AutoStepBotCommandScenario
{
    public override Guid Id { get; } = new Guid("A8FB3B10-A10A-4B50-B092-D482FE87BB11");
    public override string ScenarioCommand { get; }

    private async Task StepAction1(ITelegramBotClient bot, Update update, long chatId)
    {
        await bot.SendTextMessageAsync(chatId, BotMessages.YourName, parseMode: ParseMode.MarkdownV2);
    }
    private async Task StepAction2(ITelegramBotClient bot, Update update, long chatId)
    {
        var botUserInfo = BotHelper.GetUserInfo(update);
        var userInfo = BotDbContext.Instance.UserInfos
            .Where(i => i.UserId == chatId)
            .FirstOrDefault();
        if (userInfo == null)
        {
            var newUserInfo = new UserInfo();
            newUserInfo.UserId = botUserInfo.Id;
            newUserInfo.Name = update.Message.Text;
            newUserInfo.City = string.Empty;
            newUserInfo.Work = string.Empty;
            newUserInfo.Hobby = string.Empty;
            newUserInfo.Interests = string.Empty;
            BotDbContext.Instance.UserInfos.Add(newUserInfo);
        }
        else
        {
            userInfo.Name = update.Message.Text;
        }

        await BotDbContext.Instance.SaveChangesAsync();
        await bot.SendTextMessageAsync(chatId, BotMessages.YourCity, parseMode: ParseMode.MarkdownV2);
    }
    private async Task StepAction3(ITelegramBotClient bot, Update update, long chatId)
    {
        var userInfo = BotDbContext.Instance.UserInfos
            .Where(i => i.UserId == chatId)
            .FirstOrDefault();
        userInfo.City = update.Message.Text;

        await BotDbContext.Instance.SaveChangesAsync();
        await bot.SendTextMessageAsync(chatId, BotMessages.YourWork, parseMode: ParseMode.MarkdownV2);
    }
    private async Task StepAction4(ITelegramBotClient bot, Update update, long chatId)
    {
        var userInfo = BotDbContext.Instance.UserInfos
            .Where(i => i.UserId == chatId)
            .FirstOrDefault();
        userInfo.Work = update.Message.Text;

        await BotDbContext.Instance.SaveChangesAsync();
        await bot.SendTextMessageAsync(chatId, BotMessages.YourHobby, parseMode: ParseMode.MarkdownV2);
    }
    private async Task StepAction5(ITelegramBotClient bot, Update update, long chatId)
    {
        var userInfo = BotDbContext.Instance.UserInfos
            .Where(i => i.UserId == chatId)
            .FirstOrDefault();
        userInfo.Hobby = update.Message.Text;

        await BotDbContext.Instance.SaveChangesAsync();
        await bot.SendTextMessageAsync(chatId, BotMessages.YourInterests, parseMode: ParseMode.MarkdownV2);
    }
    private async Task StepAction6(ITelegramBotClient bot, Update update, long chatId)
    {
        var userInfo = BotDbContext.Instance.UserInfos
            .Where(i => i.UserId == chatId)
            .FirstOrDefault();
        userInfo.Interests = update.Message.Text;

        await BotDbContext.Instance.SaveChangesAsync();
        var replyMarkup = new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData(BotMessages.OkContinue));
        await bot.SendTextMessageAsync(chatId, BotMessages.MainScenbarioIsDonePart1, parseMode: ParseMode.MarkdownV2, replyMarkup: replyMarkup);
    }    
    private async Task StepAction7(ITelegramBotClient bot, Update update, long chatId)
    {
        var replyMarkup = new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData(BotMessages.OkWait));
        await bot.SendTextMessageAsync(chatId, BotMessages.MainScenbarioIsDonePart2, parseMode: ParseMode.MarkdownV2, replyMarkup: replyMarkup);
    }
    private async Task StepAction8(ITelegramBotClient bot, Update update, long chatId)
    {
        await bot.SendTextMessageAsync(chatId, BotMessages.MainScenbarioIsDonePart3, parseMode: ParseMode.MarkdownV2);
    }
    
    public MainScenario()
    {
        this.steps = new List<BotCommandScenarioStep>
        {
            new (StepAction1),
            new (StepAction2),
            new (StepAction3),
            new (StepAction4),
            new (StepAction5),
            new (StepAction6),
            new (StepAction7),
            new (StepAction8),

        }.GetEnumerator();
    }
}