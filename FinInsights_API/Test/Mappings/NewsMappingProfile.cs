using AutoMapper;
using Test.DataModels;

public class NewsMappingProfile : Profile
{
    public NewsMappingProfile()
    {
        CreateMap<MarketAuxEntity, NewsEntityDto>();

        CreateMap<MarketAuxNews, NewsDto>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Uuid))
            .ForMember(
                dest => dest.Keywords,
                opt => opt.MapFrom<KeywordsResolver>())
            .ForMember(
                dest => dest.Entities,
                opt => opt.MapFrom(src => src.Entities));

        CreateMap<MarketAuxResponse, NewsResponseDto>()
            .ForMember(
                dest => dest.News,
                opt => opt.MapFrom(src => src.Data));
    }
}