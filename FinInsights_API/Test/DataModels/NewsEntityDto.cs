public class NewsEntityDto
{
    public string Symbol { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public string Industry { get; set; } = string.Empty;

    public decimal? SentimentScore { get; set; }
}