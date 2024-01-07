using AutoMapper;
using Base.Models;
using Business.Cqrs;
using CurrencyApi.Base.Response;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;

namespace Business.Query;

public class CurrencyQueryHandler :
    IRequestHandler<GetAllSupportedCurrencies, ApiResponse<CurrencyResponse>>
{
    private readonly IMapper _mapper;

    private readonly IOptions<PositionOptions> _options;

    private readonly IUrlPaths _urlPaths;

    public CurrencyQueryHandler(IMapper mapper, IOptions<PositionOptions> options, IUrlPaths urlPaths)
    {
        _mapper = mapper;
        _options = options;
        _urlPaths = urlPaths;
    }

    public async Task<ApiResponse<CurrencyResponse>> Handle(GetAllSupportedCurrencies request,
        CancellationToken cancellationToken)
    {
        var client = new RestClient(_options.Value.Url);
        var virtualRequest = new RestRequest(_urlPaths.SupportedCurrencies);

        // Set the headers using the configuration
        virtualRequest.AddHeader("X-RapidAPI-Key", _options.Value.ApiKey);
        virtualRequest.AddHeader("X-RapidAPI-Host", _options.Value.Host);

        // Execute the request
        RestResponse response = await client.ExecuteAsync(virtualRequest, cancellationToken);

        // Check the response
        if (!response.IsSuccessful)
        {
            return new ApiResponse<CurrencyResponse>(response.ErrorMessage);
        }

        // Map the response to the ApiResponse
        if (response.Content != null)
        {
            CurrencyApiResponse? apiResponse = JsonConvert.DeserializeObject<CurrencyApiResponse>(response.Content);

            CurrencyResponse currencyData = _mapper.Map<CurrencyResponse>(apiResponse?.Symbols);
            
            return new ApiResponse<CurrencyResponse>(currencyData);
        }
        else
        {
            return new ApiResponse<CurrencyResponse>("No content");
        }
    }
}