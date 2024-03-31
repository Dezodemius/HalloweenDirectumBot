using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BotCommon.Scenarios;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DirectumCareerNightBot.Scenarios;

public class WorkInITButWantToChangeCompanyScenario : AutoStepBotCommandScenario
{
    public override Guid Id { get; } = new ("EDC574B7-60DA-494B-A6F5-EC43E2BEE923");
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
        // TODO: Расскажи в какой компании ты сейчас работаешь, на какой должности, если хочешь напиши подробнее про свой опыт
    }
    public WorkInITButWantToChangeCompanyScenario()
    {
        this.steps = new List<BotCommandScenarioStep>()
        {
            new (StepAction1),
            new (StepAction2),
            new (StepAction3),

        }.GetEnumerator();
    }
}