namespace Base.Models;

public record CurrencyApiResponse
{
    public bool Success { get; set; }
    public Dictionary<string, string> Symbols { get; set; }
}