using DigiKalaDiscounts.Models;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace DigiKalaDiscounts.Services;

public class TelegramBotService : ITelegramBotService
{
    public Task SendPhotoAsync(TelegramGroupOptions telegramGroupOptions, ProductItem productItem)
    {
        ArgumentNullException.ThrowIfNull(telegramGroupOptions);

        var telegramBotClient = new TelegramBotClient(telegramGroupOptions.AccessToken);
        return telegramBotClient.SendPhotoAsync(telegramGroupOptions.ChatId,
                                                productItem.ImageUrl ?? "",
                                                productItem.ToString(),
                                                ParseMode.Html
                                               );
    }
}
