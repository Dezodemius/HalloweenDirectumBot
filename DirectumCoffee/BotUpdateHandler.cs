using BotCommon;
using BotCommon.Repository;
using BotCommon.Scenarios;
using Newtonsoft.Json;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace DirectumCoffee;

public class BotUpdateHandler : IUpdateHandler
{
    private static readonly ILogger log = LogManager.GetCurrentClassLogger();

    private static UserScenarioRepository? _userScenarioRepository;

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        log.Trace(JsonConvert.SerializeObject(update));
        var userInfo = BotHelper.GetUserInfo(update);
        log.Info($"user: {BotHelper.GetUsername(userInfo)}, userMessage: {BotHelper.GetMessage(update)}");
        var userId = userInfo.Id;



        UserCommandScenario? userScenario = null;
        
        switch (BotHelper.GetMessage(update))
        {
            case BotChatCommands.Start:
            {
                var replyMarkup = new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData(BotMessages.GoMessage, BotChatCommands.Go));
                await botClient.SendTextMessageAsync(userId, 
                    BotMessages.BotStartMessage, 
                    cancellationToken: cancellationToken,
                    parseMode: ParseMode.MarkdownV2,
                    replyMarkup: replyMarkup);
                break;
            }
        }
        if (userScenario == null && _userScenarioRepository.TryGet(userId, out var _userScenario))
            userScenario = _userScenario;
        else
            _userScenarioRepository.AddOrReplace(userScenario);

        if (userScenario != null && !(await userScenario.Run(botClient, update, userId)))
            _userScenarioRepository.Remove(userScenario);
    }

    public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        log.Error(exception);
        Environment.Exit(0);
    }

    public BotUpdateHandler()
    {
        _userScenarioRepository = new UserScenarioRepository();
    }
}