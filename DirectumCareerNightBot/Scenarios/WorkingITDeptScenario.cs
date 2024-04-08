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

public class WorkingITDeptScenario : AutoStepBotCommandScenario
{
    public override Guid Id { get; } = new Guid("5C7B454C-0B27-48A1-9769-A1D93EB6450B");
    public override string ScenarioCommand { get; }
    
    private async Task StepAction1(ITelegramBotClient bot, Update update, long chatId)
    {
        await using var dbContext = new BotDbContext();
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
        dbContext.UserDatas.Add(userData);
        await dbContext.SaveChangesAsync();

        await bot.SendTextMessageAsync(chatId, BotMessages.IntroduceYourself);
    }
    private async Task StepAction2(ITelegramBotClient bot, Update update, long chatId)
    {
        await using var dbContext = new BotDbContext();
        var user = BotHelper.GetUserInfo(update);
        var userData = dbContext.UserDatas
            .Where(u => u.UserId == user.Id)
            .OrderByDescending(d => d.Id)
            .First();
        userData.Fullname = BotHelper.GetMessage(update);
        await dbContext.SaveChangesAsync();
        
        await bot.SendTextMessageAsync(chatId, BotMessages.HowToContact);
    }
    private async Task StepAction3(ITelegramBotClient bot, Update update, long chatId)
    {
        await using var dbContext = new BotDbContext();
        var user = BotHelper.GetUserInfo(update);
        var userData = dbContext.UserDatas
            .Where(u => u.UserId == user.Id)
            .OrderByDescending(d => d.Id)
            .First();
        userData.Contact = BotHelper.GetMessage(update);
        await dbContext.SaveChangesAsync();
        
        await bot.SendTextMessageAsync(chatId, BotMessages.TellAboutLastWork);
    }
    private async Task StepAction4(ITelegramBotClient bot, Update update, long chatId)
    {
        await using var dbContext = new BotDbContext();
        var user = BotHelper.GetUserInfo(update);
        var userData = dbContext.UserDatas
            .Where(u => u.UserId == user.Id)
            .OrderByDescending(d => d.Id)
            .First();
        userData.Experience = BotHelper.GetMessage(update);
        await dbContext.SaveChangesAsync();
        
        var sheetManager = new GoogleSheetsManager();
        sheetManager.AddUserToMatureSheet(userData.Fullname, userData.Contact, userData.SomeField, userData.Experience, userData.TelegramName);

        var buttons = new List<InlineKeyboardButton[]>
        {
            new[] { InlineKeyboardButton.WithCallbackData(BotMessages.MainMenuButton, BotChatCommands.MainMenu) }
        };
        var markup = new InlineKeyboardMarkup(buttons);
        await bot.SendTextMessageAsync(chatId, BotMessages.ThankYouInITDept, replyMarkup: markup);
    }
    public WorkingITDeptScenario()
    {
        this.steps = new List<BotCommandScenarioStep>
        {
            new (StepAction1),
            new (StepAction2),
            new (StepAction3),
            new (StepAction4),

        }.GetEnumerator();
    }
}