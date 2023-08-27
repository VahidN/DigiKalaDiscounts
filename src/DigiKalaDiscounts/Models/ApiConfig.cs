namespace DigiKalaDiscounts.Models;

public class ApiConfig
{
    public required string ItemsHistoryFileName { set; get; }

    public int ItemsHistoryInDays { set; get; }

    public int AllowedRetries { set; get; }

    public required string OutputFolderName { set; get; }

    public List<string> ApiUrls { set; get; } = new();

    public int MaxApiPagesToSearch { set; get; }

    public int TaskDelayInSeconds { set; get; }
}
