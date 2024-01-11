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
    IRequestHandler<ConvertCurrency, ApiResponse<ConvertCurrencyResponse>>,
    IRequestHandler<ConvertMultipleCurrency, ApiResponse<ConvertMultipleCurrencyResponse>>
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

    public async Task<ApiResponse<ConvertCurrencyResponse>> Handle(ConvertCurrency request,
        CancellationToken cancellationToken)
    {
        // TODO:
        // Convert decimal amount
        // Valıd but unsupported currency and invalid currency
        // Rate limiting
        // Validations are working ?
        var client = new RestClient(_options.Value.Url);

        var requestUrl = String.Format(_urlPaths.ConvertCurrency, request.Model.From, request.Model.To,
            request.Model.Amount);

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
            ConvertCurrencyJsonResponse? apiResponse =
                JsonConvert.DeserializeObject<ConvertCurrencyJsonResponse>(response.Content);

            ConvertCurrencyResponse currencyData = _mapper.Map<ConvertCurrencyResponse>(apiResponse);

            return new ApiResponse<ConvertCurrencyResponse>(currencyData);
        }
        else
        {
            return new ApiResponse<ConvertCurrencyResponse>("No content");
        }
    }

    public async Task<ApiResponse<ConvertMultipleCurrencyResponse>> Handle(ConvertMultipleCurrency request,
        CancellationToken cancellationToken)
    {
        //TODO:
        // From'da tek bir currency yer almalı
        // to ve from valid currency olmalı
        // to ve from aynı olmamalı
        var client = new RestClient(_options.Value.Url);

        var requestUrl = String.Format(_urlPaths.ConvertMultipleCurrency, request.Model.From);
        var virtualRequest = new RestRequest(requestUrl);

        virtualRequest.AddHeader("X-RapidAPI-Key", _options.Value.ApiKey);
        virtualRequest.AddHeader("X-RapidAPI-Host", _options.Value.Host);

        RestResponse<JsonNode> response = await client.ExecuteAsync<JsonNode>(virtualRequest, cancellationToken);

        if (!(bool)response.Data["success"])
        {
            return new ApiResponse<ConvertMultipleCurrencyResponse>(response.Data["error"]["info"].ToString());
        }

        if (response.Content != null)
        {
            ConvertMultipleCurrencyJsonResponse? apiResponse =
                JsonConvert.DeserializeObject<ConvertMultipleCurrencyJsonResponse>(response.Content);


            var convertedRates = new Dictionary<string, double>();
            string fromCurrencyCode = request.Model.From.ToUpper();

            double fromCurrencyRate = apiResponse.Rates.TryGetValue(fromCurrencyCode, out double fromRate)
                ? fromRate
                : 0;

            if (fromCurrencyRate == 0)
            {
                return new ApiResponse<ConvertMultipleCurrencyResponse>("Target currency rate is 0 or not found");
            }


            foreach (var rate in request.Model.To)
            {
                double toCurrencyRate = apiResponse.Rates.TryGetValue(rate.ToUpper(), out double toRate)
                    ? toRate
                    : 0;

                double convertedRate = toCurrencyRate / fromCurrencyRate;

                convertedRates.Add(rate.ToUpper(), convertedRate);
            }

            ConvertMultipleCurrencyResponse currencyData = _mapper.Map<ConvertMultipleCurrencyResponse>(convertedRates);

            return new ApiResponse<ConvertMultipleCurrencyResponse>(currencyData);
            // return new ApiResponse<ConvertMultipleCurrencyResponse>(new ConvertMultipleCurrencyResponse());
        }
        else
        {
            return new ApiResponse<ConvertMultipleCurrencyResponse>("No content");
        }
    }
}