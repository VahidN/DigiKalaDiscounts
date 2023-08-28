using DigiKalaDiscounts.Models;
using DigiKalaDiscounts.Services.Contracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DigiKalaDiscounts.Services;

public class AppPathService : IAppPathService
{
    public AppPathService(IHostEnvironment hostEnvironment,
                          IOptions<AppConfig> appConfig,
                          ILogger<AppPathService> logger)
    {
        var downloaderConfig = appConfig.Value.ApiConfig;
        var root = GetRootPath(hostEnvironment);
        logger.LogInformation("Using root: {Root}", root);

        OutputFolderPath = Path.Combine(root, downloaderConfig.OutputFolderName);
        CheckDirExists(OutputFolderPath);

        ProcessedItemsHistoryFilePath = Path.Combine(OutputFolderPath, downloaderConfig.ItemsHistoryFileName);
    }

    public string ProcessedItemsHistoryFilePath { get; }

    public string OutputFolderPath { get; }

    private static void CheckDirExists(string outputFolderPath)
    {
        if (!Directory.Exists(outputFolderPath))
        {
            Directory.CreateDirectory(outputFolderPath);
        }
    }

    private static string GetRootPath(IHostEnvironment hostEnvironment)
    {
        return hostEnvironment.ContentRootPath.Split(new[]
                                                     {
                                                         $"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}",
                                                     },
                                                     StringSplitOptions.RemoveEmptyEntries)[0];
    }
}
