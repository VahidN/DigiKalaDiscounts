using DigiKalaDiscounts.Models;
using DigiKalaDiscounts.Services.Contracts;
using Microsoft.Extensions.Options;

namespace DigiKalaDiscounts.Services;

public class ProductItemsHistoryService : IProductItemsHistoryService
{
    private readonly IOptions<AppConfig> _appConfig;
    private readonly IAppPathService _appPathService;
    private List<ProductItem> _downloadedItems = new();

    public ProductItemsHistoryService(IAppPathService appPathService,
                                      IOptions<AppConfig> appConfig)
    {
        _appPathService = appPathService;
        _appConfig = appConfig;
        LoadProcessesItems();
    }

    public void SaveChanges()
    {
        var path = _appPathService.ProcessedItemsHistoryFilePath;
        var json = JsonSerializer.Serialize(_downloadedItems,
                                            new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }

    public ProductItem? FindItem(ProductItem item)
    {
        return _downloadedItems.Find(downloadItem =>
                                         string.Equals(downloadItem.Id, item.Id, StringComparison.Ordinal));
    }

    public ProductItem? FindItem(string id)
    {
        return _downloadedItems.Find(downloadItem =>
                                         string.Equals(downloadItem.Id, id, StringComparison.Ordinal));
    }

    public bool ShouldProcessItem(ProductItem item)
    {
        var existingItem = FindItem(item);
        if (existingItem is null)
        {
            return true;
        }

        if (existingItem.IsDone)
        {
            return false;
        }

        if (existingItem.EndDate < item.EndDate)
        {
            return true;
        }

        var downloaderConfig = _appConfig.Value.ApiConfig;
        return existingItem.Retries <= downloaderConfig.AllowedRetries;
    }

    public void MarkAsCompletedItem(ProductItem item)
    {
        var existingItem = FindItem(item);
        if (existingItem is null)
        {
            item.IsDone = true;
            _downloadedItems.Add(item);
        }
        else
        {
            existingItem.IsDone = true;
        }
    }

    public void AddIncompleteItem(ProductItem item, string error)
    {
        var existingItem = FindItem(item);
        if (existingItem is null)
        {
            item.Retries = 1;
            item.Error = error;
            _downloadedItems.Add(item);
        }
        else
        {
            existingItem.Retries++;
            existingItem.Error = error;
        }
    }

    private void LoadProcessesItems()
    {
        var path = _appPathService.ProcessedItemsHistoryFilePath;
        if (!File.Exists(path))
        {
            return;
        }

        var jsonList = File.ReadAllText(path);
        var downloadedItems = JsonSerializer.Deserialize<List<ProductItem>>(jsonList,
                                                                            new JsonSerializerOptions
                                                                            {
                                                                                PropertyNameCaseInsensitive = true,
                                                                            }) ?? new List<ProductItem>();
        var maxKeepItemsDateTime = DateTime.UtcNow.AddDays(-_appConfig.Value.ApiConfig.ItemsHistoryInDays);
        _downloadedItems = downloadedItems.Where(item => item.AddDate >= maxKeepItemsDateTime).ToList();
    }
}
