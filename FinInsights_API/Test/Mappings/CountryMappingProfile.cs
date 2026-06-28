using AutoMapper;
using Test.DataModels;

public class CountryMappingProfile : Profile
{
    public CountryMappingProfile()
    {
        CreateMap<Country, CountryDto>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.CountryId))
            .ForMember(
                dest => dest.Flag,
                opt => opt.MapFrom(src => GetFlag(src.CountryCode)));
    }

    private static string GetFlag(string countryCode)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
            return "🌍";

        if (countryCode.Equals("global", StringComparison.OrdinalIgnoreCase))
            return "🌍";

        return string.Concat(
            countryCode.ToUpper()
                .Select(c => char.ConvertFromUtf32(c + 127397))
        );
    }
}