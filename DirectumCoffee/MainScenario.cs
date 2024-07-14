using BotCommon.Scenarios;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DirectumCoffee;

public class MainScenario : AutoStepBotCommandScenario
{
    public override Guid Id { get; } = new Guid("A8FB3B10-A10A-4B50-B092-D482FE87BB11");
    public override string ScenarioCommand { get; }

    private async Task StepAction1(ITelegramBotClient bot, Update update, long chatId)
    {
        
    }
    private async Task StepAction2(ITelegramBotClient bot, Update update, long chatId)
    {
        
    }
    private async Task StepAction3(ITelegramBotClient bot, Update update, long chatId)
    {
        
    }
    private async Task StepAction4(ITelegramBotClient bot, Update update, long chatId)
    {
        
    }
    private async Task StepAction5(ITelegramBotClient bot, Update update, long chatId)
    {
        
    }
    private async Task StepAction6(ITelegramBotClient bot, Update update, long chatId)
    {
        
    }

    
    public MainScenario()
    {
        this.steps = new List<BotCommandScenarioStep>
        {
            new (StepAction1),
            new (StepAction2),
            new (StepAction3),
            new (StepAction4),
            new (StepAction5),
            new (StepAction6),

        }.GetEnumerator();
    }
}