using DigiKalaDiscounts.Models;
using DigiKalaDiscounts.Services;
using DigiKalaDiscounts.Services.Contracts;

namespace DigiKalaDiscounts.Tests.Factory;

public class FakeTelegramBotService : ITelegramBotService
{
    private readonly IAppPathService _appPathService;

    public FakeTelegramBotService(IAppPathService appPathService) => _appPathService = appPathService;

    public Task SendPhotoAsync(TelegramGroupOptions telegramGroupOptions, ProductItem productItem) =>
        File.AppendAllTextAsync(Path.Combine(_appPathService.OutputFolderPath, "output.txt"),
                                $"{productItem}{Environment.NewLine}{Environment.NewLine}");
}
