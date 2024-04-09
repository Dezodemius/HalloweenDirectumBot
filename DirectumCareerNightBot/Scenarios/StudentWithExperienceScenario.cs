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

public class StudentWithExperienceScenario : AutoStepBotCommandScenario
{
    public override Guid Id { get; } = new ("A00C9F14-6B9E-4421-9D14-E73A1E73EB2F");
    public override string ScenarioCommand => string.Empty;
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

        var buttons = new List<InlineKeyboardButton[]>
        {
            new []{ InlineKeyboardButton.WithCallbackData("Да", "Yes")},
            new []{ InlineKeyboardButton.WithCallbackData("Нет", "No")}
        };
        var markup = new InlineKeyboardMarkup(buttons);
        await bot.SendTextMessageAsync(chatId, "Работал ли ты до этого в IT?",
            parseMode: ParseMode.MarkdownV2,
            replyMarkup: markup);
    }   
    private async Task StepAction25(ITelegramBotClient bot, Update update, long chatId)
    {
        var userChoice = BotHelper.GetMessage(update);
        if (userChoice == "Yes")
        {
            await bot.SendTextMessageAsync(chatId, BotMessages.IntroduceYourself,
                parseMode: ParseMode.MarkdownV2);
        }
        else if (userChoice == "No")
        {
            this.steps = new List<BotCommandScenarioStep>().GetEnumerator();
            var buttons = new List<InlineKeyboardButton[]>
            {
                new []{ InlineKeyboardButton.WithUrl(BotMessages.DirectumStudentsVK, "https://vk.com/student_directum")},
                new []{ InlineKeyboardButton.WithCallbackData(BotMessages.MainMenuButton, BotChatCommands.MainMenu)}
            };
            var markup = new InlineKeyboardMarkup(buttons);
            await bot.SendTextMessageAsync(chatId, BotMessages.TraineeITMan,
                parseMode: ParseMode.MarkdownV2,
                replyMarkup: markup);
        }
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
        
        await bot.SendTextMessageAsync(chatId, BotMessages.HowToContact,
            parseMode: ParseMode.MarkdownV2);
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
        
        await bot.SendTextMessageAsync(chatId, BotMessages.TellAboutITExpirience,
            parseMode: ParseMode.MarkdownV2);
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
        sheetManager.AddUserToInterviewSheet(userData.Fullname, userData.Contact, userData.SomeField, userData.Experience, userData.TelegramName);

        var buttons = new List<InlineKeyboardButton[]>
        {
            new[] { InlineKeyboardButton.WithUrl(BotMessages.DirectumStudentsVK, "https://vk.com/student_directum") },
            new[] { InlineKeyboardButton.WithCallbackData(BotMessages.MainMenuButton, BotChatCommands.MainMenu) }
        };
        var markup = new InlineKeyboardMarkup(buttons);
        await bot.SendTextMessageAsync(chatId, BotMessages.ThankYouAlumnus, replyMarkup: markup,
            parseMode: ParseMode.MarkdownV2);
    }
    public StudentWithExperienceScenario()
    {
        this.steps = new List<BotCommandScenarioStep>
        {
            new (StepAction1),
            new (StepAction25),
            new (StepAction2),
            new (StepAction3),
            new (StepAction4),

        }.GetEnumerator();
    }
}