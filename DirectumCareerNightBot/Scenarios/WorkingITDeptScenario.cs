using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BotCommon.Scenarios;
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
        await bot.SendTextMessageAsync(chatId, BotMessages.IntroduceYourself);
    }
    private async Task StepAction2(ITelegramBotClient bot, Update update, long chatId)
    {
        Console.WriteLine(update.Message.Text);
        await bot.SendTextMessageAsync(chatId, BotMessages.HowToContact);
    }
    private async Task StepAction3(ITelegramBotClient bot, Update update, long chatId)
    {
        Console.WriteLine(update.Message.Text);
        await bot.SendTextMessageAsync(chatId, BotMessages.TellAboutYourCompanyPlace);
    }
    private async Task StepAction4(ITelegramBotClient bot, Update update, long chatId)
    {
        Console.WriteLine(update.Message.Text);
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