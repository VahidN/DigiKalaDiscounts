namespace DigiKalaDiscounts.Services.Contracts;

public interface IAppPathService
{
    string ProcessedItemsHistoryFilePath { get; }

    string OutputFolderPath { get; }
}
