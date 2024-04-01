using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BotCommon.Scenarios;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DirectumCareerNightBot.Scenarios;

public class StudentWithExperienceScenario : AutoStepBotCommandScenario
{
    public override Guid Id { get; } = new ("A00C9F14-6B9E-4421-9D14-E73A1E73EB2F");
    public override string ScenarioCommand => string.Empty;
    private Task StepAction1(ITelegramBotClient bot, Update update, long chatid)
    {
        // TODO: Давай знакомиться, напиши свои фамилию, имя и отчество
    }
    private Task StepAction2(ITelegramBotClient bot, Update update, long chatid)
    {
        // TODO: Как удобнее будет поддерживать связь с нашим HR-ом — напиши свои телефон/почту/vk
    }
    private Task StepAction3(ITelegramBotClient bot, Update update, long chatid)
    {
        // TODO: Расскажи о своём опыты в ИТ, в какой компании работал, на какой должности
    }
    public StudentWithExperienceScenario()
    {
        this.steps = new List<BotCommandScenarioStep>()
        {
            new (StepAction1),
            new (StepAction2),
            new (StepAction3),

        }.GetEnumerator();
    }
}