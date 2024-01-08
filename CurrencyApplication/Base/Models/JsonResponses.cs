namespace Base.Models;

public record CurrencyJsonResponse
{
    public bool Success { get; set; }
    public Dictionary<string, string> Symbols { get; set; }
}


public class Info
{
    public int timestamp { get; set; }
    public double rate { get; set; }
}

public class Query
{
    public string from { get; set; }
    public string to { get; set; }
    public double amount { get; set; }
}

public class ConvertCurrencyJsonResponse
{
    public bool success { get; set; }
    public Query query { get; set; }
    public Info info { get; set; }
    public string date { get; set; }
    public double result { get; set; }
}

