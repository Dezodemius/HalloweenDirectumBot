using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BotCommon.Scenarios;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace DirectumCareerNightBot.Scenarios;

public class NotStudentButWantInITScenario : AutoStepBotCommandScenario
{
    public override Guid Id { get; } = new ("70E00609-A85A-4DB2-A518-5F3BFDA9992F");
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
        await bot.SendTextMessageAsync(chatId, BotMessages.TellAboutLastWork, ParseMode.Markdown);
    }

    private async Task StepAction4(ITelegramBotClient bot, Update update, long chatId)
    {
        Console.WriteLine(update.Message.Text);
        await bot.SendTextMessageAsync(chatId, BotMessages.WhatYouAlreadyLearned, ParseMode.Markdown);
    }

    private async Task StepAction5(ITelegramBotClient bot, Update update, long chatId)
    {
        Console.WriteLine(update.Message.Text);
        var buttons = new List<InlineKeyboardButton[]>
        {
            new[] { InlineKeyboardButton.WithCallbackData(BotMessages.MainMenuButton, BotChatCommands.MainMenu) }
        };
        var markup = new InlineKeyboardMarkup(buttons);
        await bot.SendTextMessageAsync(chatId, BotMessages.ThankYouNotStudent, ParseMode.Markdown, replyMarkup: markup);
    }

    public NotStudentButWantInITScenario()
    {
        this.steps = new List<BotCommandScenarioStep>()
        {
            new (StepAction1),
            new (StepAction2),
            new (StepAction3),
            new (StepAction4),
            new (StepAction5),

        }.GetEnumerator();
    }
}