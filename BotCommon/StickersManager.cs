using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HalloweenDirectumBot;

public static class StickersManager
{
  private static Sticker[] _stickers;

  public static void InitializeStickerPack(ITelegramBotClient bot, string stickerPackName)
  {
    _stickers = bot.GetStickerSetAsync(stickerPackName).Result.Stickers;
  }

  public static async void SendStickerAsync(ITelegramBotClient bot, long chatId, string stickerEmoji)
  {
    if (_stickers == null)
      return;
    await bot.SendStickerAsync(chatId, new InputFileId(_stickers.First(x => x.Emoji == stickerEmoji).FileId));
  }
}