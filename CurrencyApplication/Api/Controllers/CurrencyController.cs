using Base.Models;
using Business.Cqrs;
using CurrencyApi.Base.Response;
using Microsoft.AspNetCore.Mvc;
using MediatR;


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
}
