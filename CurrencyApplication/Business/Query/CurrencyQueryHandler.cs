using System.Dynamic;
using System.Net;
using System.Text.Json.Nodes;
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
    IRequestHandler<GetAllSupportedCurrencies, ApiResponse<CurrencyResponse>>,
    IRequestHandler<ConvertCurrency, ApiResponse<ConvertCurrencyResponse>>
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

        virtualRequest.AddHeader("X-RapidAPI-Key", _options.Value.ApiKey);
        virtualRequest.AddHeader("X-RapidAPI-Host", _options.Value.Host);

        RestResponse<JsonNode> response = await client.ExecuteAsync<JsonNode>(virtualRequest, cancellationToken);
        // var dynamicObject = JsonSerializer.Deserialize<JsonNode>(response.Content);
        
        if (!(bool)response.Data["success"])
        {
            return new ApiResponse<CurrencyResponse>(response.Data["error"]["info"].ToString());
        }

        if (response.Content != null)
        {
            CurrencyJsonResponse? apiResponse = JsonConvert.DeserializeObject<CurrencyJsonResponse>(response.Content);

            CurrencyResponse currencyData = _mapper.Map<CurrencyResponse>(apiResponse?.Symbols);
            
            return new ApiResponse<CurrencyResponse>(currencyData);
        }
        else
        {
            return new ApiResponse<CurrencyResponse>("No content");
        }
    }

    public async Task<ApiResponse<ConvertCurrencyResponse>> Handle(ConvertCurrency request, CancellationToken cancellationToken)
    {
        // TODO:
        // Convert decimal amount
        // Valıd but unsupported currency and invalid currency
        // Rate limiting
        // Validations are working ?
        var client = new RestClient(_options.Value.Url);
        
        var requestUrl = String.Format(_urlPaths.ConvertCurrency, request.Model.From, request.Model.To, request.Model.Amount);
        
        var virtualRequest = new RestRequest(requestUrl);

        virtualRequest.AddHeader("X-RapidAPI-Key", _options.Value.ApiKey);
        virtualRequest.AddHeader("X-RapidAPI-Host", _options.Value.Host);

        RestResponse<JsonNode> response = await client.ExecuteAsync<JsonNode>(virtualRequest, cancellationToken);

        if (!(bool)response.Data["success"])
        {
            return new ApiResponse<ConvertCurrencyResponse>(response.Data["error"]["info"].ToString());
        }

        if (response.Content != null)
        {
            ConvertCurrencyJsonResponse? apiResponse = JsonConvert.DeserializeObject<ConvertCurrencyJsonResponse>(response.Content);
        
            ConvertCurrencyResponse currencyData = _mapper.Map<ConvertCurrencyResponse>(apiResponse);
            
            return new ApiResponse<ConvertCurrencyResponse>(currencyData);
        }
        else
        {
            return new ApiResponse<ConvertCurrencyResponse>("No content");
        }
    
    }
}