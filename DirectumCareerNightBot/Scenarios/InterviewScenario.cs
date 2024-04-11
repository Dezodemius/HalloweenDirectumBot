using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotCommon;
using BotCommon.Scenarios;
using DirectumCareerNightBot.GoogleSheets;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace DirectumCareerNightBot.Scenarios;

public class InterviewScenario : AutoStepBotCommandScenario
{
    public override Guid Id { get; } = new ("EDC574B7-60DA-494B-A6F5-EC43E2BEE923");
    public override string ScenarioCommand => string.Empty;
    private async Task StepAction1(ITelegramBotClient bot, Update update, long chatId)
    {
        var botUserInfo = BotHelper.GetUserInfo(update);
        var userData = new UserData
        {
            TelegramName = string.IsNullOrEmpty(botUserInfo.Username)
                ? $"{botUserInfo.FirstName} {botUserInfo.LastName}"
                : botUserInfo.Username,
            UserId = botUserInfo.Id,
            Fullname = string.Empty,
            Contact = string.Empty,
            SomeField = string.Empty,
            Experience = string.Empty
        };
        BotDbContext.Instance.UserDatas.Add(userData);
        await BotDbContext.Instance.SaveChangesAsync();

        await bot.EditMessageTextAsync(
            chatId,
            update.CallbackQuery.Message.MessageId,
            BotMessages.IntroduceYourself,
            parseMode: ParseMode.MarkdownV2);
    }
    private async Task StepAction2(ITelegramBotClient bot, Update update, long chatId)
    {
        var user = BotHelper.GetUserInfo(update);
        var userData = BotDbContext.Instance.UserDatas
            .Where(u => u.UserId == user.Id)
            .OrderByDescending(d => d.Id)
            .First();
        userData.Fullname = BotHelper.GetMessage(update);
        await BotDbContext.Instance.SaveChangesAsync();

        await bot.SendTextMessageAsync(chatId, BotMessages.HowToContact,
            parseMode: ParseMode.MarkdownV2);
    }
    private async Task StepAction3(ITelegramBotClient bot, Update update, long chatId)
    {
        var user = BotHelper.GetUserInfo(update);
        var userData = BotDbContext.Instance.UserDatas
            .Where(u => u.UserId == user.Id)
            .OrderByDescending(d => d.Id)
            .First();
        userData.Contact = BotHelper.GetMessage(update);
        await BotDbContext.Instance.SaveChangesAsync();

        var buttons = new List<InlineKeyboardButton[]>()
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(BotMessages.Yes, BotChatCommands.WorkInIt),
                InlineKeyboardButton.WithCallbackData(BotMessages.No, BotChatCommands.NoWorkInIt),
            }
        };
        var markup = new InlineKeyboardMarkup(buttons);
        await bot.SendTextMessageAsync(chatId, BotMessages.WorkedInIT,
            replyMarkup: markup,
            parseMode: ParseMode.MarkdownV2);
    }
    private async Task StepAction4(ITelegramBotClient bot, Update update, long chatId)
    {
        var userChoice = BotHelper.GetMessage(update);
        if (userChoice == BotChatCommands.WorkInIt)
        {
            await bot.SendTextMessageAsync(chatId, BotMessages.TellAboutYourCompany,
                parseMode: ParseMode.MarkdownV2);
        }
        else if (userChoice == BotChatCommands.NoWorkInIt)
        {
            var newScenario = new WantToITScenario();
            this.steps = new List<BotCommandScenarioStep>
            {
                new (newScenario.StepAction3),
                new (newScenario.StepAction4),
                new (newScenario.StepAction5),

            }.GetEnumerator();
            this.ExecuteStep(bot, update, chatId);
        }
    }
    private async Task StepAction5(ITelegramBotClient bot, Update update, long chatId)
    {
        var user = BotHelper.GetUserInfo(update);
        var userData = BotDbContext.Instance.UserDatas
            .Where(u => u.UserId == user.Id)
            .OrderByDescending(d => d.Id)
            .First();
        userData.Experience = BotHelper.GetMessage(update);
        await BotDbContext.Instance.SaveChangesAsync();
        
        var sheetManager = new GoogleSheetsManager();
        sheetManager.AddUserToInterviewSheet(userData.Fullname, userData.Contact, userData.Experience, userData.TelegramName);

        var buttons = new List<InlineKeyboardButton[]>
        {
            new[] { InlineKeyboardButton.WithCallbackData(BotMessages.MainMenuButton, BotChatCommands.MainMenu) }
        };
        var markup = new InlineKeyboardMarkup(buttons);
        await bot.SendTextMessageAsync(chatId, BotMessages.ThankYouInIT, replyMarkup: markup,
            parseMode: ParseMode.MarkdownV2);
    }
    public InterviewScenario()
    {
        this.steps = new List<BotCommandScenarioStep>
        {
            new (StepAction1),
            new (StepAction2),
            new (StepAction3),
            new (StepAction4),
            new (StepAction5),

        }.GetEnumerator();
    }
}