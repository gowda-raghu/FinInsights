using Test.DataModels;
public class StockDetailDto
{
    public StockMetaDto Meta { get; set; } = new();

    public List<StockPriceDto> Data { get; set; } = new();
}