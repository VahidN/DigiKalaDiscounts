using DigiKalaDiscounts.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DigiKalaDiscounts.Tests.Factory;

internal static class TestsAppFactory
{
    private static readonly Lazy<IHost> HostProvider = new(GetHost, LazyThreadSafetyMode.ExecutionAndPublication);

    /// <summary>
    ///     A lazy loaded thread-safe singleton
    /// </summary>
    public static IHost Host { get; } = HostProvider.Value;

    public static T GetRequiredService<T>() where T : notnull => Host.Services.GetRequiredService<T>();

    private static IHost GetHost()
    {
        var builder = Microsoft.Extensions.Hosting.Host
                               .CreateDefaultBuilder()
                               .ConfigureServices((context, services) =>
                                                  {
                                                      var (_, httpClientBuilder) =
                                                          services.AddServices(context.Configuration);
                                                      services.AddSingleton<FakeHttpMessageHandler>();
                                                      httpClientBuilder.AddHttpMessageHandler<FakeHttpMessageHandler>();
                                                      services
                                                          .AddSingleton<ITelegramBotService, FakeTelegramBotService>();
                                                  });
        var app = builder.Build();
        return app;
    }
}
