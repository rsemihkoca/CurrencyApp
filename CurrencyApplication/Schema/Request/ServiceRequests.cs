namespace Schema.Request;
using Base.Models;
public record ConvertCurrencyRequest(string From, string To, double Amount);

public record ConvertMultipleCurrencyRequest(List<string> To, string From);