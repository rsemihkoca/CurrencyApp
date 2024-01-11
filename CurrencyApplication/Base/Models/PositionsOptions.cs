namespace Base.Models;

public class PositionOptions
{
    public const string Position = "CurrencyProvider";
    
    public string Url { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
}