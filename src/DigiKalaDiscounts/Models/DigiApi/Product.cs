namespace DigiKalaDiscounts.Models.DigiApi;

public class Product
{
    [JsonPropertyName("id")] public long Id { set; get; }

    [JsonPropertyName("status")] public string? Status { set; get; }

    [JsonPropertyName("title_fa")] public string? TitleFa { set; get; }

    [JsonPropertyName("url")] public Url? Url { set; get; }

    [JsonPropertyName("images")] public Images? Images { set; get; }

    [JsonPropertyName("default_variant")] public DefaultVariant? DefaultVariant { set; get; }
}
