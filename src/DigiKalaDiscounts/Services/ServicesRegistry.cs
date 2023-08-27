using DigiKalaDiscounts.Models;
using DigiKalaDiscounts.Services.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DigiKalaDiscounts.Services;

public static class ServicesRegistry
{
    public static (IServiceCollection, IHttpClientBuilder) AddServices(this IServiceCollection serviceCollection,
                                                                       IConfiguration configuration)
    {
        serviceCollection.TryAddSingleton<ITelegramBotService, TelegramBotService>();
        serviceCollection.TryAddSingleton<IFetchProductItemsService, FetchProductItemsService>();
        serviceCollection.TryAddSingleton<IProductItemsHistoryService, ProductItemsHistoryService>();
        serviceCollection.TryAddSingleton<IAppPathService, AppPathService>();
        serviceCollection.TryAddSingleton<IAppRunnerService, AppRunnerService>();
        serviceCollection.Configure<AppConfig>(configuration.Bind);
        var httpClientBuilder = serviceCollection.AddHttpClient(configuration);
        return (serviceCollection, httpClientBuilder);
    }
}
