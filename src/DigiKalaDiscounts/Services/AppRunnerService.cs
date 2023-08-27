using CommandLine;
using DigiKalaDiscounts.Models;
using DigiKalaDiscounts.Services.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DigiKalaDiscounts.Services;

/// <summary>
///     Defines the entry point of the application
/// </summary>
public class AppRunnerService : IAppRunnerService
{
    private readonly IOptions<AppConfig> _appConfig;
    private readonly IFetchProductItemsService _fetchProductItemsService;
    private readonly ILogger<AppRunnerService> _logger;
    private readonly IProductItemsHistoryService _productItemsHistoryService;
    private readonly ITelegramBotService _telegramBotService;

    public AppRunnerService(IProductItemsHistoryService productItemsHistoryService,
                            IFetchProductItemsService fetchProductItemsService,
                            ITelegramBotService telegramBotService,
                            IOptions<AppConfig> appConfig,
                            ILogger<AppRunnerService> logger)
    {
        _productItemsHistoryService = productItemsHistoryService;
        _fetchProductItemsService = fetchProductItemsService;
        _telegramBotService = telegramBotService;
        _appConfig = appConfig;
        _logger = logger;
    }

    public async Task<AppRunnerResults> StartAsync(string[] args)
    {
        var results = new AppRunnerResults();
        await Parser.Default.ParseArguments<TelegramGroupOptions>(args)
                    .WithParsedAsync(async telegramGroupOptions => results = await ProcessAsync(telegramGroupOptions));
        return results;
    }

    private async Task<AppRunnerResults> ProcessAsync(TelegramGroupOptions telegramGroupOptions)
    {
        var results = new AppRunnerResults();
        try
        {
            var productItems = await _fetchProductItemsService.GetProductItemsAsync();
            var productItemsCount = productItems.Count;
            results.TotalItemsCount = productItemsCount;
            if (productItemsCount == 0)
            {
                _logger.LogInformation("There is nothing to process.");
                SaveAll();
                return results;
            }

            var index = 0;

            foreach (var productItem in productItems)
            {
                index++;

                try
                {
                    _logger.LogInformation("Processing[{Index}/{Count}] {Title}",
                                           index,
                                           productItemsCount,
                                           productItem.Title);

                    if (!_productItemsHistoryService.ShouldProcessItem(productItem))
                    {
                        _logger.LogInformation("Skipped processing an already processed item: `{Product}`",
                                               productItem.Title);
                        results.SkippedItemsCount++;
                        continue;
                    }

                    await _telegramBotService.SendPhotoAsync(telegramGroupOptions, productItem);
                    _productItemsHistoryService.MarkAsCompletedItem(productItem);
                    results.CompletedItemsCount++;

                    await Task.Delay(TimeSpan.FromSeconds(_appConfig.Value.ApiConfig.TaskDelayInSeconds));
                }
                catch (Exception ex)
                {
                    _productItemsHistoryService.AddIncompleteItem(productItem, ex.Message);
                    results.IncompleteItemsCount++;
                    _logger.LogCritical(ex, "Couldn't process `{Title}`", productItem.Title);
                }
            }

            SaveAll();
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Fatal error");
        }

        return results;
    }

    private void SaveAll()
    {
        _productItemsHistoryService.SaveChanges();
    }
}
