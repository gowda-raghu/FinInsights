using AutoMapper;
using Test.DataModels;


public class KeywordsResolver
    : IValueResolver<MarketAuxNews, NewsDto, List<string>>
{
    public List<string> Resolve(
        MarketAuxNews source,
        NewsDto destination,
        List<string> destMember,
        ResolutionContext context)
    {
        if (string.IsNullOrWhiteSpace(source.Keywords))
        {
            return [];
        }

        return source.Keywords
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .ToList();
    }
}