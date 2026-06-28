using Test.DataModels;
using Test.DataLayer;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YahooFinanceApi;
public class StockService : IStockService
{

    private readonly RaghuDbContext _context;
    private readonly IEmailClient _emailClient;
    private readonly HttpClient _httpClient;

    private readonly string finnHubbaseUrl = "https://finnhub.io/api/v1";

    private readonly string alphaUrl = "https://www.alphavantage.co/query";

    private readonly string ApiKey;
    private readonly string AplhaApiKey;
    private readonly IConfiguration _config;

    private readonly IMapper _mapper;

    public StockService(HttpClient httpClient, RaghuDbContext context, IEmailClient emailClient, IConfiguration config, IMapper mapper)
    {
        _httpClient = httpClient;
        _context = context;
        _emailClient = emailClient;
        _config = config;
        _mapper = mapper;
        ApiKey = _config["FinnHub:ApiKey"];
        AplhaApiKey = _config["AlphaVantage:ApiKey"];
    }




    public async Task<List<StockSearchResultDto>> SearchStocks(string searchText)
    {
        var finnHuburl = $"{finnHubbaseUrl}/search?q={searchText}&token={ApiKey}";

        var response = await _httpClient.GetFromJsonAsync<SearchStocksResponseDto>(finnHuburl);
        return response?.Result ?? [];
    }


    public async Task<StockDetailDto> GetStockDetails(string symbol)
    {
        var securities = await Yahoo
            .Symbols(symbol)
            .Fields(
                Field.ShortName,
                Field.Currency,
                Field.Exchange,
                Field.RegularMarketPrice,
                Field.RegularMarketDayHigh,
                Field.RegularMarketDayLow,
                Field.RegularMarketVolume,
                Field.FiftyTwoWeekHigh,
                Field.FiftyTwoWeekLow
            )
            .QueryAsync();

        if (!securities.ContainsKey(symbol))
            throw new Exception("Stock not found.");

        var stock = securities[symbol];

        var history = (await Yahoo.GetHistoricalAsync(
                symbol,
                DateTime.UtcNow.AddYears(-1),
                DateTime.UtcNow,
                Period.Daily))
            .OrderBy(x => x.DateTime)
            .ToList();

        if (history.Count == 0)
            throw new Exception("No historical data available.");

        decimal previousClose = history.Count > 1
            ? history[^2].Close
            : history[^1].Close;

        var result = new StockDetailDto
        {
            Meta = new StockMetaDto
            {
                Symbol = symbol,
                CompanyName = stock[Field.ShortName]?.ToString() ?? "",
                Currency = stock[Field.Currency]?.ToString() ?? "",
                Exchange = stock[Field.Exchange]?.ToString() ?? "",

                CurrentPrice = Convert.ToDecimal(stock[Field.RegularMarketPrice]),
                PreviousClose = previousClose,

                DayHigh = Convert.ToDecimal(stock[Field.RegularMarketDayHigh]),
                DayLow = Convert.ToDecimal(stock[Field.RegularMarketDayLow]),

                FiftyTwoWeekHigh = Convert.ToDecimal(stock[Field.FiftyTwoWeekHigh]),
                FiftyTwoWeekLow = Convert.ToDecimal(stock[Field.FiftyTwoWeekLow]),

                Volume = Convert.ToInt64(stock[Field.RegularMarketVolume])
            },

            Data = history.Select(x => new StockPriceDto
            {
                Date = x.DateTime,
                Open = x.Open,
                High = x.High,
                Low = x.Low,
                Close = x.Close,
                Volume = x.Volume
            }).ToList()
        };

        return result;
    }
    // public async Task<StockDetailDto> GetStockDetails(string symbol)
    // {
    //     var securities = await Yahoo
    // .Symbols("AAPL")
    // .Fields(
    //     Field.ShortName,
    //     Field.RegularMarketPrice,
    //     Field.RegularMarketDayHigh,
    //     Field.RegularMarketDayLow,
    //     Field.RegularMarketVolume,
    //     Field.FiftyTwoWeekHigh,
    //     Field.FiftyTwoWeekLow,
    //     Field.MarketCap
    // )
    // .QueryAsync();

    //     var stock = securities["AAPL"];

    //     Console.WriteLine(stock[Field.ShortName]);
    //     Console.WriteLine(stock[Field.RegularMarketPrice]);

    //     return null;
    //     // var url =
    //     //     $"https://query1.finance.yahoo.com/v8/finance/chart/{symbol}?range=1y&interval=1d";

    //     // var response =
    //     //     await _httpClient.GetFromJsonAsync<YahooChartResponse>(url);

    //     // if (response?.Chart?.Result == null || response.Chart.Result.Count == 0)
    //     //     throw new Exception("Stock not found.");

    //     // var result = response.Chart.Result.First();
    //     // var quote = result.Indicators.Quote.First();

    //     // var stock = new StockDetailDto
    //     // {
    //     //     Meta = new StockMetaDto
    //     //     {
    //     //         Symbol = result.Meta.Symbol,
    //     //         CompanyName = result.Meta.LongName,
    //     //         Currency = result.Meta.Currency,
    //     //         Exchange = result.Meta.FullExchangeName,
    //     //         CurrentPrice = result.Meta.RegularMarketPrice,
    //     //         PreviousClose = result.Meta.PreviousClose,
    //     //         DayHigh = result.Meta.RegularMarketDayHigh,
    //     //         DayLow = result.Meta.RegularMarketDayLow,
    //     //         FiftyTwoWeekHigh = result.Meta.FiftyTwoWeekHigh,
    //     //         FiftyTwoWeekLow = result.Meta.FiftyTwoWeekLow,
    //     //         Volume = result.Meta.RegularMarketVolume
    //     //     }
    //     // };

    //     // for (int i = 0; i < result.Timestamp.Count; i++)
    //     // {
    //     //     stock.Data.Add(new StockPriceDto
    //     //     {
    //     //         Date = DateTimeOffset.FromUnixTimeSeconds(result.Timestamp[i]).DateTime,
    //     //         Open = quote.Open[i] ?? 0,
    //     //         High = quote.High[i] ?? 0,
    //     //         Low = quote.Low[i] ?? 0,
    //     //         Close = quote.Close[i] ?? 0,
    //     //         Volume = quote.Volume[i] ?? 0
    //     //     });
    //     // }

    //     // return stock;
    // }
    // // public async Task<StockDetailDto> GetStockDetails(string symbol)
    // {
    //     var from =
    //         DateTimeOffset.UtcNow
    //             .AddYears(-1)
    //             .ToUnixTimeSeconds();

    //     var to =
    //         DateTimeOffset.UtcNow
    //             .ToUnixTimeSeconds();

    //     var profileUrl =
    //         $"{finnHubbaseUrl}/stock/profile2?symbol={symbol}&token={ApiKey}";

    //     var quoteUrl =
    //         $"{finnHubbaseUrl}/quote?symbol={symbol}&token={ApiKey}";

    //     var chartUrl =
    //         $"{alphaUrl}?function=TIME_SERIES_DAILY_ADJUSTED&symbol={symbol}&outputsize=compact&apikey={AplhaApiKey}";

    //     var profileTask =
    //         _httpClient.GetFromJsonAsync<FinnhubProfileResponse>(
    //             profileUrl);

    //     var quoteTask =
    //         _httpClient.GetFromJsonAsync<FinnhubQuoteResponse>(
    //             quoteUrl);

    //     var chartTask =
    //          _httpClient.GetFromJsonAsync<AlphaVantageResponse>(chartUrl);

    //     await Task.WhenAll(
    //         profileTask,
    //         quoteTask,
    //         chartTask);

    //     var profile = await profileTask;
    //     var quote = await quoteTask;
    //     var chartResponse = await chartTask;

    //     var chartDto = new StockChartDto();

    //     foreach (var item in chartResponse.TimeSeriesDaily
    //                                       .OrderBy(x => x.Key))
    //     {
    //         chartDto.TimeStamps.Add(
    //             DateTime.Parse(item.Key)
    //                 .ToUniversalTime()
    //                 .Ticks);

    //         chartDto.ClosePrices.Add(
    //             decimal.Parse(item.Value.Close));
    //     }

    //     return new StockDetailDto
    //     {
    //         Profile = new StockProfileDto
    //         {
    //             Name = profile?.Name ?? "",
    //             Ticker = profile?.Ticker ?? "",
    //             Country = profile?.Country ?? "",
    //             Currency = profile?.Currency ?? "",
    //             Exchange = profile?.Exchange ?? "",
    //             Industry = profile?.FinnhubIndustry ?? "",
    //             Logo = profile?.Logo ?? "",
    //             Website = profile?.Weburl ?? "",
    //             IpoDate = profile?.Ipo ?? "",
    //             MarketCapitalization =
    //                 profile?.MarketCapitalization ?? 0
    //         },

    //         Quote = new StockQuoteDto
    //         {
    //             CurrentPrice = quote?.C ?? 0,
    //             Change = quote?.D ?? 0,
    //             ChangePercent = quote?.Dp ?? 0,
    //             High = quote?.H ?? 0,
    //             Low = quote?.L ?? 0,
    //             Open = quote?.O ?? 0,
    //             PreviousClose = quote?.Pc ?? 0
    //         },

    //         Chart = new StockChartDto
    //         {
    //             TimeStamps = chartDto?.TimeStamps ?? [],
    //             ClosePrices = chartDto?.ClosePrices ?? []
    //         }
    //     };
    // }

}