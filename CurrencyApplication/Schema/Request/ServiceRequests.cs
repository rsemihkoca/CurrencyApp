namespace Schema.Request;

public record ConvertCurrencyRequest(string From, string To, double Amount);

public record ConvertMultipleCurrencyRequest(string From, string To);