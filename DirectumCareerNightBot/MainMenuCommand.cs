using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace DirectumCareerNightBot;

public static class MainMenuCommand
{
    public static IEnumerable<InlineKeyboardButton[]> GetMainMenuButtons()
    {
        return new List<InlineKeyboardButton[]>
        {
            new[] { InlineKeyboardButton.WithCallbackData(BotMessages.Student, BotChatCommands.Student) },
            new[] { InlineKeyboardButton.WithCallbackData(BotMessages.NotStudent, BotChatCommands.NotStudent) },
            new[] { InlineKeyboardButton.WithCallbackData(BotMessages.RafflePrizes, BotChatCommands.RafflePrizes) }
        };
    }
    public static InlineKeyboardMarkup GetMainMenuInlineMarkup()
    {
        // var inlineButtons = new List<InlineKeyboardButton[]>
        // {
        //     new []
        //     {
        //         InlineKeyboardButton.WithCallbackData(BotMessages.WantToPractice, BotChatCommands.WantToPractice),
        //         InlineKeyboardButton.WithCallbackData(BotMessages.StudentWithExperience, BotChatCommands.StudentWithExperience),
        //     },
        //     new []
        //     {
        //         InlineKeyboardButton.WithCallbackData(BotMessages.NotStudentButWantInIT, BotChatCommands.NotStudentButWantInIT),
        //         InlineKeyboardButton.WithCallbackData(BotMessages.WorkerInITDept, BotChatCommands.WorkerInITDept),
        //     },
        //     new []
        //     {
        //         InlineKeyboardButton.WithCallbackData(BotMessages.WorkInITButWantToChangeCompany, BotChatCommands.WorkInITButWantToChangeCompany),
        //     },
        //     new[]
        //     {
        //         InlineKeyboardButton.WithCallbackData(BotMessages.Directum15Questions, BotChatCommands.Directum15Questions),
        //         InlineKeyboardButton.WithCallbackData(BotMessages.RafflePrizes, BotChatCommands.RafflePrizes),
        //     },
        //     new []
        //     {
        //         InlineKeyboardButton.WithUrl(BotMessages.Career, "https://career.directum.ru/vacancy"),
        //     },
        //     new []
        //     {                
        //         InlineKeyboardButton.WithUrl(BotMessages.DirectumStudentsVK, "https://vk.com/student_directum"),
        //     }
        // };
        return new InlineKeyboardMarkup(GetMainMenuButtons());
    }
}