namespace CurrencyApi.Base.Response;

public record CurrencyResponse
{
    public Dictionary<string, string> Data { get; set; }
}