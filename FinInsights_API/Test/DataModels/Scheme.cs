using System.Text.Json.Serialization;

public class Scheme
{
    [JsonPropertyName("schemeCode")]
    public int SchemeCode { get; set; }

    [JsonPropertyName("schemeName")]
    public string SchemeName { get; set; }

    [JsonPropertyName("isinGrowth")]
    public string IsinGrowth { get; set; }

    [JsonPropertyName("isinDivReinvestment")]
    public string IsinDivReinvestment { get; set; }

}