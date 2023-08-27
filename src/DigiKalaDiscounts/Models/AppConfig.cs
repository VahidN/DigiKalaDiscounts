namespace DigiKalaDiscounts.Models;

public class AppConfig
{
    public required HttpClientConfig HttpClientConfig { set; get; }

    public required ApiConfig ApiConfig { set; get; }
}
