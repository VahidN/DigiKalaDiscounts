using DigiKalaDiscounts.Models;

namespace DigiKalaDiscounts.Services;

public interface ITelegramBotService
{
    Task SendPhotoAsync(TelegramGroupOptions telegramGroupOptions, ProductItem productItem);
}
