using System.Text.Json.Serialization;
using Test.DataModels;
public class AlphaVantageResponse
{
    [JsonPropertyName("Time Series (Daily)")]
    public Dictionary<string, DailyPriceDto> TimeSeriesDaily { get; set; }
        = new();
}

