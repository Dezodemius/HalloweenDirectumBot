using System;
using System.Timers;
using Newtonsoft.Json;
using NLog;
using Telegram.Bot;

namespace BotCommon.KeepAlive;

/// <summary>
/// Bot alive keeper.
/// </summary>
public class BotKeepAlive
{
  #region Fields and Properties

  /// <summary>
  /// Logger.
  /// </summary>
  private static readonly ILogger _log = LogManager.GetCurrentClassLogger();
  
  /// <summary>
  /// Default time out to send keep alive.
  /// </summary>
  private static readonly TimeSpan _defaultTimeout = TimeSpan.FromMinutes(30);
  
  /// <summary>
  /// Keep alive request timer.
  /// </summary>
  private readonly Timer _timer;

  #endregion

  #region Methods

  /// <summary>
  /// Stop sending keep alive.
  /// </summary>
  public void StopKeepAlive()
  {
    _timer.Stop();
    _timer.Dispose();
    _log.Info("Keep Alive Stoped");
  }

  /// <summary>
  /// Start send keep alive.
  /// </summary>
  public void StartKeepAlive()
  {
    _timer.Start();
    _log.Info("Keep Alive Started");
  }

  #endregion

  #region Constructors

  /// <summary>
  /// Constructor.
  /// </summary>
  /// <param name="bot">Telegram bot client.</param>
  public BotKeepAlive(ITelegramBotClient bot)
    : this(bot, _defaultTimeout)
  {
  }

  /// <summary>
  /// Constructor.
  /// </summary>
  /// <param name="bot">Telegram bot client.</param>
  /// <param name="timeout">Keep alive timeout.</param>
  public BotKeepAlive(ITelegramBotClient bot, TimeSpan timeout)
  {
    _timer = new Timer(timeout);
    _timer.AutoReset = true;
    _timer.Elapsed += (_, _) =>
    {
      var botInfo = bot.GetMeAsync().Result;
      _log.Info($"KeepAlive: {JsonConvert.SerializeObject(botInfo)}");
    };
  }

  #endregion
}