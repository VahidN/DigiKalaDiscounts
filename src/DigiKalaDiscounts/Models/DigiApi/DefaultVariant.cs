namespace DigiKalaDiscounts.Models.DigiApi;

public class DefaultVariant
{
    [JsonPropertyName("price")] public Price? Price { set; get; }
}
