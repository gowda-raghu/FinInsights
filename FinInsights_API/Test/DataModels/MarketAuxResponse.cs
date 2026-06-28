
using System.Text.Json.Serialization;

public class MarketAuxResponse
{
    [JsonPropertyName("data")]
    public List<MarketAuxNews> Data { get; set; } = [];
}