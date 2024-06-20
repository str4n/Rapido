using Rapido.Services.Wallets.Domain.Wallets.Exceptions;

namespace Rapido.Services.Wallets.Domain.Wallets.Money;

public sealed record Currency
{
    private static readonly HashSet<string> AllowedValues = new()
    {
        "PLN", "USD", "EUR", "GBP"
    };
    
    public string Value { get; }

    public Currency(string value)
    {
        value = value.ToUpperInvariant();
        
        if (string.IsNullOrWhiteSpace(value) || value.Length != 3)
        {
            throw new InvalidCurrencyException();
        }

        if (!AllowedValues.Contains(value))
        {
            throw new UnsupportedCurrencyException();
        }

        Value = value;
    }

    public static implicit operator string(Currency currency) => currency.Value;
    public static implicit operator Currency(string currency) => new(currency);

    public static Currency PLN() => new("PLN");
    public static Currency USD() => new("USD");
    public static Currency EUR() => new("EUR");
    public static Currency GBP() => new("GBP");
    

    public override string ToString() => Value;
}