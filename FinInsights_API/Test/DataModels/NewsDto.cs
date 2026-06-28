public class NewsDto
{
    public string Id { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Source { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public string? ImageUrl { get; set; }

    public DateTime PublishedAt { get; set; }

    public List<string> Keywords { get; set; } = [];

    public List<NewsEntityDto> Entities { get; set; } = [];
}