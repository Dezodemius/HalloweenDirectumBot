using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCommon;

/// <summary>
/// Sticker manager.
/// </summary>
public class StickersManager
{
  /// <summary>
  /// Stickers.
  /// </summary>
  private Sticker[] _stickers;

  public void InitializeStickerPack(ITelegramBotClient bot, string stickerPackName)
  {
    _stickers = bot.GetStickerSetAsync(stickerPackName).Result.Stickers;
  }

  /// <summary>
  /// Send  sticker.
  /// </summary>
  /// <param name="bot">Telegram bot client.</param>
  /// <param name="userId">Telegram user id.</param>
  /// <param name="stickerEmoji">Sticker emoji.</param>
  public async void SendStickerAsync(ITelegramBotClient bot, long userId, string stickerEmoji)
  {
    if (_stickers == null)
      return;
    await bot.SendStickerAsync(userId, new InputFileId(_stickers.First(x => x.Emoji == stickerEmoji).FileId));
  }
}