public class FundComparisonDto
{
    public FundDto Fund1 { get; set; } = new();

    public FundDto Fund2 { get; set; } = new();

    public WinnerDto Winner { get; set; } = new();
}