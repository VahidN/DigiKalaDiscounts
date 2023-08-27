namespace DigiKalaDiscounts.Models.DigiApi;

public class DigiData
{
    [JsonPropertyName("products")] public IList<Product>? Products { set; get; }
}
