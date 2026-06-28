using Test.DataModels;
public class YahooChartDto
{
    public List<YahooChartResultDto> Result { get; set; } = new();

    public object? Error { get; set; }
}