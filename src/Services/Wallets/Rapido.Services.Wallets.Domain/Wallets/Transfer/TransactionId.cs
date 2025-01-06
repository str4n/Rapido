namespace Rapido.Services.Wallets.Domain.Wallets.Transfer;

// correlation id
public sealed record TransactionId(string Value)
{
    private const string AvailableCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const int Length = 24;
    
    public TransactionId() : this(Generate())
    {
    }

    public static TransactionId Create() => new(Generate());
    
    public static implicit operator string(TransactionId id) => id.Value;
    
    public static implicit operator TransactionId(string id) => new(id);

    private static string Generate()
        => new string(Enumerable.Repeat(AvailableCharacters, Length).Select(x => x[new Random().Next(x.Length)])
            .ToArray());
}