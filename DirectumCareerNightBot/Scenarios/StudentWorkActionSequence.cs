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

public class StudentWorkActionSequence : AutoStepBotActionSequence
{
    public override Guid Id { get; } = new ("A00C9F14-6B9E-4421-9D14-E73A1E73EB2F");
    private async Task StepAction1(ITelegramBotClient bot, Update update, long chatId)
    {
        var botUserInfo = BotHelper.GetUserFromUpdate(update);
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

        var buttons = new List<InlineKeyboardButton[]>
        {
            new []{ InlineKeyboardButton.WithCallbackData("Да", "Yes")},
            new []{ InlineKeyboardButton.WithCallbackData("Нет", "No")}
        };
        var markup = new InlineKeyboardMarkup(buttons);
        await bot.EditMessageTextAsync(
            chatId,
            update.CallbackQuery.Message.MessageId,
            "Работал ли ты до этого в IT?",
            parseMode: ParseMode.MarkdownV2,
            replyMarkup: markup);
    }   
    private async Task StepAction25(ITelegramBotClient bot, Update update, long chatId)
    {
        var userChoice = BotHelper.GetMessageText(update);
        if (userChoice == "Yes")
        {
            await bot.SendTextMessageAsync(chatId, BotMessages.IntroduceYourself,
                parseMode: ParseMode.MarkdownV2);
        }
        else if (userChoice == "No")
        {
            sequenceActions = new List<SequenceAction>().GetEnumerator();
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
        var user = BotHelper.GetUserFromUpdate(update);
        var userData = BotDbContext.Instance.UserDatas
            .Where(u => u.UserId == user.Id)
            .OrderByDescending(d => d.Id)
            .First();
        userData.Fullname = BotHelper.GetMessageText(update);
        await BotDbContext.Instance.SaveChangesAsync();
        
        await bot.SendTextMessageAsync(chatId, BotMessages.HowToContact,
            parseMode: ParseMode.MarkdownV2);
    }
    private async Task StepAction3(ITelegramBotClient bot, Update update, long chatId)
    {
        var user = BotHelper.GetUserFromUpdate(update);
        var userData = BotDbContext.Instance.UserDatas
            .Where(u => u.UserId == user.Id)
            .OrderByDescending(d => d.Id)
            .First();
        userData.Contact = BotHelper.GetMessageText(update);
        await BotDbContext.Instance.SaveChangesAsync();
        
        await bot.SendTextMessageAsync(chatId, BotMessages.TellAboutITExpirience,
            parseMode: ParseMode.MarkdownV2);
    }

    private async Task StepAction4(ITelegramBotClient bot, Update update, long chatId)
    {
        var user = BotHelper.GetUserFromUpdate(update);
        var userData = BotDbContext.Instance.UserDatas
            .Where(u => u.UserId == user.Id)
            .OrderByDescending(d => d.Id)
            .First();
        userData.Experience = BotHelper.GetMessageText(update);
        await BotDbContext.Instance.SaveChangesAsync();

        var sheetManager = new GoogleSheetsManager();
        sheetManager.AddUserToInterviewSheet(userData.Fullname, userData.Contact, userData.Experience, userData.TelegramName);

        var buttons = new List<InlineKeyboardButton[]>
        {
            new[] { InlineKeyboardButton.WithUrl(BotMessages.DirectumStudentsVK, "https://vk.com/student_directum") },
            new[] { InlineKeyboardButton.WithCallbackData(BotMessages.MainMenuButton, BotChatCommands.MainMenu) }
        };
        var markup = new InlineKeyboardMarkup(buttons);
        await bot.SendTextMessageAsync(chatId, BotMessages.ThankYouAlumnus, replyMarkup: markup,
            parseMode: ParseMode.MarkdownV2);
    }
    public StudentWorkActionSequence()
    {
        sequenceActions = new List<SequenceAction>
        {
            new (StepAction1),
            new (StepAction25),
            new (StepAction2),
            new (StepAction3),
            new (StepAction4),

        }.GetEnumerator();
    }
}