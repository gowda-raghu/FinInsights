public class SearchStocksResponseDto
{
    public int Count { get; set; }

    public List<StockSearchResultDto> Result { get; set; } = [];
}