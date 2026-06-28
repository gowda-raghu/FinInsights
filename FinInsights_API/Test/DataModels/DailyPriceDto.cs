using System.Text.Json.Serialization;

public class DailyPriceDto
{
    [JsonPropertyName("4. close")]
    public string Close { get; set; } = string.Empty;
}