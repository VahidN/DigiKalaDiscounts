namespace DigiKalaDiscounts.Models.DigiApi;

public class RootObject
{
    [JsonPropertyName("status")] public int Status { set; get; }
    [JsonPropertyName("data")] public DigiData? Data { set; get; }
}
