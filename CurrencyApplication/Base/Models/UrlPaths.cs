namespace Base.Models;



public interface IUrlPaths
{
    string SupportedCurrencies { get; }
}

public class UrlPaths : IUrlPaths
{
    public string SupportedCurrencies { get; } = "/Symbols";
}
