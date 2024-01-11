namespace CurrencyApi.Base.Response;

public record CurrencyResponse
{
    public Dictionary<string, string> Data { get; set; }
}

public record ConvertCurrencyResponse
{
    public decimal Amount { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public decimal ConvertedAmount { get; set; }
}

public record ConvertMultipleCurrencyResponse
{
    public Dictionary<string, string> Data { get; set; }

}