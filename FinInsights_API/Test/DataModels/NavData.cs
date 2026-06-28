using System.Text.Json.Serialization;

public class NavData
{
    [JsonPropertyName("date")]
    public string Date { get; set; }

    [JsonPropertyName("nav")]
    public string Nav { get; set; }
}