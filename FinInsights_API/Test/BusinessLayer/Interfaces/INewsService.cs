using Test.DataModels;

public interface INewsService
{
    Task<NewsResponseDto> GetNewsAsync(string countryCode);

    Task<List<CountryDto>> GetCountriesList();
}