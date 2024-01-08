namespace Base.Models;



public interface IUrlPaths
{
    string SupportedCurrencies { get; }
    string ConvertCurrency { get; }
}

public class UrlPaths : IUrlPaths
{
    public string SupportedCurrencies { get; } = "/symbols";
    public string ConvertCurrency { get; } = "/convert?from={0:string}&to={1:string}&amount={2}";
}
