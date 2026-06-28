using Test.DataModels;
using Test.DataLayer;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
public class NewsService : INewsService
{
    private readonly RaghuDbContext _context;
    private readonly IEmailClient _emailClient;
    private readonly HttpClient _httpClient;

    private readonly string baseUrl = "https://api.marketaux.com/v1/news/all";

    private readonly string ApiKey;

    private readonly IConfiguration _config;

    private readonly IMapper _mapper;

    public NewsService(HttpClient httpClient, RaghuDbContext context, IEmailClient emailClient, IConfiguration config, IMapper mapper)
    {
        _httpClient = httpClient;
        _context = context;
        _emailClient = emailClient;
        _config = config;
        _mapper = mapper;
        ApiKey = _config["MarketAeux:ApiKey"];
    }

    public async Task<NewsResponseDto> GetNewsAsync(string countryCode)
    {
        var publishedAfter = DateTime.UtcNow
            .AddDays(-1)
            .ToString("yyyy-MM-ddTHH:mm");

        var url =
            $"{baseUrl}" +
            $"?countries={countryCode}" +
            $"&filter_entities=true" +
            $"&limit=30" +
            $"&published_after={publishedAfter}" +
            $"&api_token={ApiKey}";
        Console.WriteLine(url);

        var response = await _httpClient.GetAsync(url);

        var marketAuxResponse =
            await response.Content.ReadFromJsonAsync<MarketAuxResponse>();
        var newsResponseDto = _mapper.Map<NewsResponseDto>(marketAuxResponse);
        return newsResponseDto;
    }


    public async Task<List<CountryDto>> GetCountriesList()
    {
        var countrylist = await _context.Countries!.ToListAsync();
        var countries = await _context.Countries
    .OrderBy(x => x.CountryName)
    .ToListAsync();

        var response = _mapper.Map<List<CountryDto>>(countries);
        return response;
        
    }
}