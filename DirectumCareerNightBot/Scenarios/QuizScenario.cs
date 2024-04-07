using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BotCommon.Scenarios;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DirectumCareerNightBot.Scenarios;

public class QuizScenario : AutoStepBotCommandScenario
{
    public override Guid Id { get; } = new Guid("C2181931-E2A2-409D-9E9B-220177C40556");
    public override string ScenarioCommand { get; }
    private async Task StepAction1(ITelegramBotClient bot, Update update, long chatId)
    {
        await bot.SendTextMessageAsync(chatId, BotMessages.IntroduceYourself, ParseMode.Markdown);
    }
    private async Task StepAction2(ITelegramBotClient bot, Update update, long chatId)
    {
        await bot.SendTextMessageAsync(chatId, BotMessages.IntroduceYourself, ParseMode.Markdown);
    }
    private async Task StepAction3(ITelegramBotClient bot, Update update, long chatId)
    {
        await bot.SendTextMessageAsync(chatId, BotMessages.IntroduceYourself, ParseMode.Markdown);
    }
    private async Task StepAction4(ITelegramBotClient bot, Update update, long chatId)
    {
        await bot.SendTextMessageAsync(chatId, BotMessages.IntroduceYourself, ParseMode.Markdown);
    }
    private async Task StepAction5(ITelegramBotClient bot, Update update, long chatId)
    {
        await bot.SendTextMessageAsync(chatId, BotMessages.IntroduceYourself, ParseMode.Markdown);
    }
    private async Task StepAction6(ITelegramBotClient bot, Update update, long chatId)
    {
        await bot.SendTextMessageAsync(chatId, BotMessages.IntroduceYourself, ParseMode.Markdown);
    }
    private async Task StepAction7(ITelegramBotClient bot, Update update, long chatId)
    {
        await bot.SendTextMessageAsync(chatId, BotMessages.IntroduceYourself, ParseMode.Markdown);
    }
    public QuizScenario()
    {
        this.steps = new List<BotCommandScenarioStep>()
        {
            new (StepAction1),
            new (StepAction2),
            new (StepAction3),
            new (StepAction4),
            new (StepAction5),
            new (StepAction6),
            new (StepAction7),

        }.GetEnumerator();
    }
}