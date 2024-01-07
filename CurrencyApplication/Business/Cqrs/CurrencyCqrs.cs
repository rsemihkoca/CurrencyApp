using Base.Models;
using CurrencyApi.Base.Response;
using MediatR;

namespace Business.Cqrs;


public record GetAllSupportedCurrencies : IRequest<ApiResponse<CurrencyResponse>>;
