using System.Text.Json.Serialization;

public class Meta
{
    [JsonPropertyName("fund_house")]
    public string FundHouse { get; set; }

    [JsonPropertyName("scheme_type")]
    public string SchemeType { get; set; }

    [JsonPropertyName("scheme_category")]
    public string SchemeCategory { get; set; }

    [JsonPropertyName("scheme_code")]
    public int SchemeCode { get; set; }

    [JsonPropertyName("scheme_name")]
    public string SchemeName { get; set; }

    [JsonPropertyName("isin_growth")]
    public string? IsinGrowth { get; set; }

    [JsonPropertyName("isin_div_reinvestment")]
    public string? IsinDivReinvestment { get; set; }
}