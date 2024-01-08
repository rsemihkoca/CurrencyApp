using Base.Models;
using Business.Cqrs;
using CurrencyApi.Base.Response;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Schema.Request;


namespace Api;

[Route("api/[controller]")]
[ApiController]
public class CurrencyController : ControllerBase
{
    private readonly IMediator _mediator;
    public CurrencyController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<ApiResponse<CurrencyResponse>> Get()
    {
        var operation = new GetAllSupportedCurrencies();
        var result = await _mediator.Send(operation);
        return result;
    }
    
    [HttpPost("Convert")]
    public async Task<ApiResponse<ConvertCurrencyResponse>> Post([FromBody] ConvertCurrencyRequest model)
    {
        var operation = new ConvertCurrency(model);
        var result = await _mediator.Send(operation);
        return result;
    }
}
