using Test.DataModels;

public interface IStockService
{
    Task<List<StockSearchResultDto>> SearchStocks(string searchText);

    Task<StockDetailDto> GetStockDetails(string symbol);

}