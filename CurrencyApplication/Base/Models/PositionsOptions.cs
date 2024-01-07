namespace Base.Models;


public class PositionOptions
{
    public const string Position = "CurrencyProvider";
    
    public string Url { get; set; } = String.Empty;
    public string ApiKey { get; set; } = String.Empty;
    public string Host { get; set; } = String.Empty;
}