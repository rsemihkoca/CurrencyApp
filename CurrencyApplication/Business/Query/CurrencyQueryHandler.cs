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
    IRequestHandler<ConvertCurrency, ApiResponse<ConvertCurrencyResponse>>,
    IRequestHandler<MultipleCurrencyRates, ApiResponse<ConvertMultipleCurrencyResponse>>

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
        RestResponse<JsonNode> response = await client.ExecuteAsync<JsonNode>(virtualRequest, cancellationToken);
        // var dynamicObject = JsonSerializer.Deserialize<JsonNode>(response.Content);
        
        // Check the response
        if (!(bool)response.Data["success"])
        {
            return new ApiResponse<CurrencyResponse>(response.Data["error"]["info"].ToString());
        }

        // Map the response to the ApiResponse
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
    
    public async Task<ApiResponse<ConvertMultipleCurrencyResponse>> Handle(MultipleCurrencyRates request, CancellationToken cancellationToken)
    {
        var client = new RestClient(_options.Value.Url);
        var requestUrl = string.Format(_urlPaths.ExchangeRateForSelectedCurrency, request.Model.From, request.Model.To);
        var virtualRequest = new RestRequest(requestUrl);
        
        // Set the headers using the configuration
        virtualRequest.AddHeader("X-RapidAPI-Key", _options.Value.ApiKey);
        virtualRequest.AddHeader("X-RapidAPI-Host", _options.Value.Host);

        // Execute the request
        RestResponse<JsonNode> response = await client.ExecuteAsync<JsonNode>(virtualRequest, cancellationToken);
        // var dynamicObject = JsonSerializer.Deserialize<JsonNode>(response.Content);
        
        // Check the response
        if (!(bool)response.Data["success"])
        {
            return new ApiResponse<ConvertMultipleCurrencyResponse>(response.Data["error"]["info"].ToString());
        }
        
        // Map the response to the ApiResponse
        if (response.Content != null)
        {
            var apiResponse = JsonConvert.DeserializeObject<ConvertMultipleCurrencyJsonResponse>(response.Content);
            if (apiResponse.Rates != null)
            {
                foreach (var x in apiResponse.Rates)
                {
                    if (!(request.Model.To.Contains(x.Key)))
                    {
                        if (!(request.Model.From.Contains(x.Key)))
                        {
                            apiResponse.Rates.Remove(x.Key);
                        }
                    }
                }
            }
            
            var currencyRates = new Dictionary<string, string>();
            
            var fromRate = apiResponse.Rates[request.Model.From];
            foreach (var x in apiResponse.Rates)
            {
                if (x.Key != request.Model.From)
                {
                    currencyRates.Add(x.Key, (Convert.ToDouble(x.Value)/Convert.ToDouble(fromRate)).ToString());
                }
            }

            var currencyData = _mapper.Map<ConvertMultipleCurrencyResponse>(currencyRates);
            
            return new ApiResponse<ConvertMultipleCurrencyResponse>(currencyData);
        }
        else
        {
            return new ApiResponse<ConvertMultipleCurrencyResponse>("No content");
        }
    }

    public async Task<ApiResponse<ConvertCurrencyResponse>> Handle(ConvertCurrency request, CancellationToken cancellationToken)
    {
        // Convert decimal amount
        // Valıd but unsupported currency and invalid currency
        // Rate limiting
        // Validations are working ?
        var client = new RestClient(_options.Value.Url);
        var requestUrl = string.Format(_urlPaths.ConvertCurrency, request.Model.From, request.Model.To, request.Model.Amount);
        var virtualRequest = new RestRequest(requestUrl);

        // Set the headers using the configuration
        virtualRequest.AddHeader("X-RapidAPI-Key", _options.Value.ApiKey);
        virtualRequest.AddHeader("X-RapidAPI-Host", _options.Value.Host);

        // Execute the request
        RestResponse<JsonNode> response = await client.ExecuteAsync<JsonNode>(virtualRequest, cancellationToken);

        // Check the response
        if (!(bool)response.Data["success"])
        {
            return new ApiResponse<ConvertCurrencyResponse>(response.Data["error"]["info"].ToString());
        }

        // Map the response to the ApiResponse
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