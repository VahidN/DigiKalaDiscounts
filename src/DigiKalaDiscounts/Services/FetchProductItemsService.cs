using DigiKalaDiscounts.Models;
using DigiKalaDiscounts.Models.DigiApi;
using DigiKalaDiscounts.Services.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DigiKalaDiscounts.Services;

public class FetchProductItemsService : IFetchProductItemsService
{
    private readonly IOptions<AppConfig> _appConfig;
    private readonly ApiHttpClientService _appHttpClientService;
    private readonly ILogger<FetchProductItemsService> _logger;

    public FetchProductItemsService(ApiHttpClientService appHttpClientService,
                                    IOptions<AppConfig> appConfig,
                                    ILogger<FetchProductItemsService> logger)
    {
        _appHttpClientService = appHttpClientService;
        _appConfig = appConfig;
        _logger = logger;
    }

    public async Task<List<ProductItem>> GetProductItemsAsync()
    {
        var results = new List<ProductItem>();
        var maxPages = _appConfig.Value.ApiConfig.MaxApiPagesToSearch;

        foreach (var apiUrl in _appConfig.Value.ApiConfig.ApiUrls)
        {
            for (var page = 1; page <= maxPages; page++)
            {
                var root = await GetDataRootAsync(apiUrl, page);
                if (root == null)
                {
                    continue;
                }

                if (root.Status != (int)HttpStatusCode.OK)
                {
                    continue;
                }

                var dataProducts = root.Data?.Products;
                if (dataProducts == null || dataProducts.Count == 0)
                {
                    break;
                }

                var now = DateTime.UtcNow;
                results.AddRange(dataProducts.Where(IsItemMarketable)
                                             .Select(product => CreateProductItem(product, now)));
            }
        }

        return results.Where(productItem => productItem.DiscountPercent > 0).OrderByDescending(productItem => productItem.Discount).ToList();
    }

    private static ProductItem CreateProductItem(Product product, DateTime now)
    {
        var price = product.DefaultVariant?.Price;
        var priceTimer = price?.Timer ?? 0;
        return new ProductItem
               {
                   Id = product.Id.ToString(CultureInfo.InvariantCulture),
                   AddDate = now,
                   Retries = 0,
                   IsDone = false,
                   Error = "",
                   Title = product.TitleFa,
                   Url = product.Url?.Uri,
                   ImageUrl = product.Images?.Main?.Url?.FirstOrDefault() ?? "",
                   SellingPrice = price?.SellingPrice / 10 ?? 0,
                   RrpPrice = price?.RrpPrice / 10 ?? 0,
                   SoldPercentage = price?.SoldPercentage ?? 0,
                   DiscountPercent = price?.DiscountPercent ?? 0,
                   Timer = priceTimer,
               };
    }

    private static bool IsItemMarketable(Product product) =>
        string.Equals(product.Status, "marketable", StringComparison.OrdinalIgnoreCase);

    private async Task<RootObject?> GetDataRootAsync(string apiUrl, int page)
    {
        var url = apiUrl.Replace("$page",
                                 page.ToString(CultureInfo.InvariantCulture),
                                 StringComparison.OrdinalIgnoreCase);

        try
        {
            var jsonList = await _appHttpClientService.DownloadPageAsync(url);
            jsonList = jsonList.Replace("[]", "null", StringComparison.Ordinal);
            var root = JsonSerializer.Deserialize<RootObject>(jsonList,
                                                              new JsonSerializerOptions
                                                              {
                                                                  PropertyNameCaseInsensitive = true,
                                                                  NumberHandling = JsonNumberHandling
                                                                      .AllowReadingFromString,
                                                              });
            return root;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Couldn't process `{Url}`", url);
            return null;
        }
    }
}
