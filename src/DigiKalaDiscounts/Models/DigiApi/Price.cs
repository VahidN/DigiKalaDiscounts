namespace DigiKalaDiscounts.Models.DigiApi;

public class Price
{
    [JsonPropertyName("selling_price")] public long SellingPrice { set; get; }
    [JsonPropertyName("rrp_price")] public long RrpPrice { set; get; }
    [JsonPropertyName("timer")] public long Timer { set; get; }
    [JsonPropertyName("discount_percent")] public long DiscountPercent { set; get; }

    [JsonPropertyName("sold_percentage")] public long SoldPercentage { set; get; }
}
