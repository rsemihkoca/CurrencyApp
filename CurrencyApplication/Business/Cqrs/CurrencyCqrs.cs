using Base.Models;
using CurrencyApi.Base.Response;
using MediatR;
using Schema.Request;

namespace Business.Cqrs;


public record GetAllSupportedCurrencies : IRequest<ApiResponse<CurrencyResponse>>;

public record ConvertCurrency(ConvertCurrencyRequest Model) : IRequest<ApiResponse<ConvertCurrencyResponse>>;

public record ConvertMultipleCurrency(ConvertMultipleCurrencyRequest Model) : IRequest<ApiResponse<ConvertMultipleCurrencyResponse>>;
//default olarak from eur. to kısmı boş ise hepsini dönsün.

