namespace Rapido.Services.Wallets.Domain.Owners.Owner;

public sealed class CorporateOwner : Owner
{
    public TaxId TaxId { get; private set; }
    
    public CorporateOwner(OwnerName name, TaxId taxId, DateTime createdAt) : base(name, createdAt)
    {
        TaxId = taxId;
    }

    public CorporateOwner(OwnerId id, OwnerName name, TaxId taxId, OwnerState state, DateTime createdAt) : base(id, name, createdAt)
    {
        TaxId = taxId;
        State = state;
    }
}