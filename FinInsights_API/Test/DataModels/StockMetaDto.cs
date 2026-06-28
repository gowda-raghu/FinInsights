public class StockMetaDto
{
    public string Symbol { get; set; } = string.Empty;

    public string CompanyName { get; set; } = string.Empty;

    public string Currency { get; set; } = string.Empty;

    public string Exchange { get; set; } = string.Empty;

    public string InstrumentType { get; set; } = string.Empty;

    public decimal CurrentPrice { get; set; }

    public decimal PreviousClose { get; set; }

    public decimal DayHigh { get; set; }

    public decimal DayLow { get; set; }

    public decimal FiftyTwoWeekHigh { get; set; }

    public decimal FiftyTwoWeekLow { get; set; }

    public long Volume { get; set; }

    public string MarketState { get; set; } = string.Empty;

    public string TimeZone { get; set; } = string.Empty;
}