using Test.DataModels;

public class YahooChartResultDto
{
    public YahooMetaDto Meta { get; set; } = new();

    public List<long> Timestamp { get; set; } = new();

    public YahooIndicatorsDto Indicators { get; set; } = new();
}