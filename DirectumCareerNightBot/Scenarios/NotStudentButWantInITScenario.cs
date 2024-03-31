using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BotCommon.Scenarios;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DirectumCareerNightBot.Scenarios;

public class NotStudentButWantInITScenario : AutoStepBotCommandScenario
{
    public override Guid Id { get; } = new ("70E00609-A85A-4DB2-A518-5F3BFDA9992F");
    public override string ScenarioCommand => string.Empty;

    private Task StepAction1(ITelegramBotClient bot, Update update, long chatid)
    {
        // TODO: Давай знакомиться, напиши свои фамилию, имя и отчество
    }
    private Task StepAction2(ITelegramBotClient bot, Update update, long chatid)
    {
        // TODO: Как удобнее будет поддерживать связь с нашим HR-ом — напиши свои телефон/почту
    }
    private Task StepAction3(ITelegramBotClient bot, Update update, long chatid)
    {
        // TODO: Расскажи о своём последнем месте работы, опыте
    }

    private Task StepAction4(ITelegramBotClient bot, Update update, long chatid)
    {
        // TODO: что уже изучал в ИТ
    }

    public NotStudentButWantInITScenario()
    {
        this.steps = new List<BotCommandScenarioStep>()
        {
            new (StepAction1),
            new (StepAction2),
            new (StepAction3),
            new (StepAction4),

        }.GetEnumerator();
    }
}