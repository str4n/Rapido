using System.Globalization;

namespace Rapido.Services.Wallets.Domain.Wallets.Money;

public sealed record Amount(double Value)
{
    public static Amount Zero => new((double)0);
    
    public static implicit operator double(Amount value) => value.Value;
    public static implicit operator Amount(double value) => new(value);

    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
}