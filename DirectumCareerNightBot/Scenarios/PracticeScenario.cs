using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BotCommon.Scenarios;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DirectumCareerNightBot.Scenarios;

public class PracticeScenario : AutoStepBotCommandScenario
{
    public override Guid Id { get; } = new Guid("87036BA3-710B-4742-86E0-A1FD8F699741");
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
        // TODO: Какое направление тебе интересно: Программирование Тестирование Маркетинг Продажи Другое (сам вписывает)
    }
    private Task StepAction4(ITelegramBotClient bot, Update update, long chatid)
    {
        // TODO: Расскажи про свой опыт в программировании/ тестировании/ маркетинге/ продажах (в зависимости от того, что выбрал)
    }

    public PracticeScenario()
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