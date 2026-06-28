using Test.DataModels;
public class FundResponse
{
    public Meta Meta { get; set; }
    public List<NavData> Data { get; set; }
    public string Status { get; set; }
}