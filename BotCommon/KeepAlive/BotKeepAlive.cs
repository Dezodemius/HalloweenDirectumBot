using System;
using System.Timers;
using Newtonsoft.Json;
using NLog;
using Telegram.Bot;

namespace BotCommon.KeepAlive;

public class BotKeepAlive
{
  private static readonly ILogger log = LogManager.GetCurrentClassLogger();
  private static readonly TimeSpan _defaultTimeout = TimeSpan.FromMinutes(30);
  private readonly Timer _timer;

  public void StopKeepAlive()
  {
    _timer.Stop();
    _timer.Dispose();
    log.Info("Keep Alive Stoped");
  }

  public void StartKeepAlive()
  {
    _timer.Start();
    log.Info("Keep Alive Started");
  }

  public BotKeepAlive(ITelegramBotClient bot)
      : this(bot, _defaultTimeout)
  {
  }

  public BotKeepAlive(ITelegramBotClient bot, TimeSpan timeout)
  {
    _timer = new Timer(timeout);
    _timer.AutoReset = true;
    _timer.Elapsed += (_, _) =>
    {
      var botInfo = bot.GetMeAsync().Result;
      log.Info($"KeepAlive: {JsonConvert.SerializeObject(botInfo)}");
    };
  }
}