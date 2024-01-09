using System.Text.Json.Serialization;

namespace Base.Models;

public record CurrencyJsonResponse
{
    public bool Success { get; set; }
    public Dictionary<string, string> Symbols { get; set; }
}
public class Info
{
    [JsonPropertyName("timestamp")]
    public int Timestamp { get; set; }
    
    [JsonPropertyName("rate")]
    public double Rate { get; set; }
}

public class Query
{
    [JsonPropertyName("from")]
    public string From { get; set; }

    [JsonPropertyName("to")]
    public string To { get; set; }

    [JsonPropertyName("amount")]
    public double Amount { get; set; }
}

public class ConvertCurrencyJsonResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("query")]
    public Query Query { get; set; }

    [JsonPropertyName("info")]
    public Info Info { get; set; }

    [JsonPropertyName("date")]
    public string Date { get; set; }

    [JsonPropertyName("result")]
    public double Result { get; set; }
}


