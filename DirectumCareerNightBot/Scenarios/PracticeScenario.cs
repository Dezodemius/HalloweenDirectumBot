using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BotCommon.Scenarios;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace DirectumCareerNightBot.Scenarios;

public class PracticeScenario : AutoStepBotCommandScenario
{
    public override Guid Id { get; } = new Guid("87036BA3-710B-4742-86E0-A1FD8F699741");
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
        var buttons = new List<KeyboardButton[]>
        {
            new []{ new KeyboardButton(BotMessages.Programming)},
            new []{ new KeyboardButton(BotMessages.Testing)},
            new []{ new KeyboardButton(BotMessages.Marketing)},
            new []{ new KeyboardButton(BotMessages.Sails)},
            new []{ new KeyboardButton(BotMessages.Support)},
            new []{ new KeyboardButton(BotMessages.Analitycs)},
            new []{ new KeyboardButton(BotMessages.Other)},
        };
        var markup = new ReplyKeyboardMarkup(buttons);
        await bot.SendTextMessageAsync(chatId, BotMessages.InterestingDirection, ParseMode.Markdown, replyMarkup: markup);
    }
    private async Task StepAction4(ITelegramBotClient bot, Update update, long chatId)
    {
        Console.WriteLine(update.Message.Text);
        await bot.SendTextMessageAsync(chatId, BotMessages.TellAboutChosenDirection, ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
    }
    private async Task StepAction5(ITelegramBotClient bot, Update update, long chatId)
    {
        Console.WriteLine(update.Message.Text);
        var buttons = new List<InlineKeyboardButton[]>
        {
            new[] { InlineKeyboardButton.WithCallbackData(BotMessages.MainMenuButton, BotChatCommands.MainMenu) }
        };
        var markup = new InlineKeyboardMarkup(buttons);
        await bot.SendTextMessageAsync(chatId, BotMessages.ThankYouPractice, ParseMode.Markdown, replyMarkup: markup);
    }

    public PracticeScenario()
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