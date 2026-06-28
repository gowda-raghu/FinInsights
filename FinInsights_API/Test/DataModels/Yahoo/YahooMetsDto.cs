using System.Text.Json.Serialization;
using Test.DataModels;

public class YahooMetaDto
{
    [JsonPropertyName("currency")]
    public string Currency { get; set; } = "";

    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = "";

    [JsonPropertyName("exchangeName")]
    public string ExchangeName { get; set; } = "";

    [JsonPropertyName("fullExchangeName")]
    public string FullExchangeName { get; set; } = "";

    [JsonPropertyName("instrumentType")]
    public string InstrumentType { get; set; } = "";

    [JsonPropertyName("regularMarketPrice")]
    public decimal RegularMarketPrice { get; set; }

    [JsonPropertyName("regularMarketVolume")]
    public long RegularMarketVolume { get; set; }

    [JsonPropertyName("regularMarketDayHigh")]
    public decimal RegularMarketDayHigh { get; set; }

    [JsonPropertyName("regularMarketDayLow")]
    public decimal RegularMarketDayLow { get; set; }

    [JsonPropertyName("previousClose")]
    public decimal PreviousClose { get; set; }

    [JsonPropertyName("fiftyTwoWeekHigh")]
    public decimal FiftyTwoWeekHigh { get; set; }

    [JsonPropertyName("fiftyTwoWeekLow")]
    public decimal FiftyTwoWeekLow { get; set; }

    [JsonPropertyName("shortName")]
    public string ShortName { get; set; } = "";

    [JsonPropertyName("longName")]
    public string LongName { get; set; } = "";
}