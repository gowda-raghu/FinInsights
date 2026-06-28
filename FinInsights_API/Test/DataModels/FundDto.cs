public class FundDto
{
    public string SchemeName { get; set; } = string.Empty;

    public string FundHouse { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public decimal LatestNav { get; set; }

    public string LatestNavDate { get; set; } = string.Empty;

    public decimal ReturnPercentage { get; set; }

    public int TotalRecords { get; set; }
}