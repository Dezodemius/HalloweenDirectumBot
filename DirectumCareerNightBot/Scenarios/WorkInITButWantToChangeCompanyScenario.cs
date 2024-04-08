using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BotCommon.Scenarios;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace DirectumCareerNightBot.Scenarios;

public class WorkInITButWantToChangeCompanyScenario : AutoStepBotCommandScenario
{
    public override Guid Id { get; } = new ("EDC574B7-60DA-494B-A6F5-EC43E2BEE923");
    public override string ScenarioCommand => string.Empty;
    private async Task StepAction1(ITelegramBotClient bot, Update update, long chatId)
    {
        await bot.SendTextMessageAsync(chatId, BotMessages.IntroduceYourself, ParseMode.Markdown);
    }
    private async Task StepAction2(ITelegramBotClient bot, Update update, long chatId)
    {
        Console.WriteLine(update.Message.Text);
        await bot.SendTextMessageAsync(chatId, BotMessages.HowToContact, ParseMode.Markdown);
    }
    private async Task StepAction3(ITelegramBotClient bot, Update update, long chatId)
    {
        Console.WriteLine(update.Message.Text);
        await bot.SendTextMessageAsync(chatId, BotMessages.TellAboutYourCompany, ParseMode.Markdown);
    }
    private async Task StepAction4(ITelegramBotClient bot, Update update, long chatId)
    {
        Console.WriteLine(update.Message.Text);
        var buttons = new List<InlineKeyboardButton[]>
        {
            new[] { InlineKeyboardButton.WithCallbackData(BotMessages.MainMenuButton, BotChatCommands.MainMenu) }
        };
        var markup = new InlineKeyboardMarkup(buttons);
        await bot.SendTextMessageAsync(chatId, BotMessages.ThankYouInIT, ParseMode.Markdown, replyMarkup: markup);
    }
    public WorkInITButWantToChangeCompanyScenario()
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