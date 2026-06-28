using Test.DataModels;

public class YahooQuoteDto
{
    public List<decimal?> Open { get; set; } = new();

    public List<decimal?> High { get; set; } = new();

    public List<decimal?> Low { get; set; } = new();

    public List<decimal?> Close { get; set; } = new();

    public List<long?> Volume { get; set; } = new();
}