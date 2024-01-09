using AutoMapper;
using Base.Models;
using CurrencyApi.Base.Response;

namespace Business.Mapper;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
    
        CreateMap<Dictionary<string, string>, CurrencyResponse>()
            .ForMember(dest =>dest.Data, opt => opt.MapFrom(src => src));

        CreateMap<ConvertCurrencyJsonResponse, ConvertCurrencyResponse>()
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Query.Amount))
            .ForMember(dest => dest.From, opt => opt.MapFrom(src => src.Query.From))
            .ForMember(dest => dest.To, opt => opt.MapFrom(src => src.Query.To))
            .ForMember(dest => dest.ConvertedAmount, opt => opt.MapFrom(src => src.Result));
    }
}