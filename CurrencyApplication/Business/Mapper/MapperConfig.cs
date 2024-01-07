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
        // CreateMap<KeyValuePair<string, string>, CurrencyResponse>()
        // .ForMember(x => x.Code, opt => opt.MapFrom(src => src.Key))
        // .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Value));
        // CreateMap<KeyValuePair<string, string>, CurrencyResponse>()
        //     .ForMember(x => x.Code, opt => opt.MapFrom(src => src.Key))
        //     .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Value));
        // CreateMap<KeyValuePair<string, string>, CurrencyResponse>();
    }
}