namespace Rapido.Services.Wallets.Domain.Owners.Owner;

public sealed class IndividualOwner : Owner
{
    public OwnerFullName FullName { get; private set; }
    
    public IndividualOwner(OwnerName name, OwnerFullName fullName, DateTime createdAt) : base(name, createdAt)
    {
        FullName = fullName;
    }

    public IndividualOwner(OwnerId id, OwnerName name, OwnerFullName fullName, DateTime createdAt) : base(id, name, createdAt)
    {
        FullName = fullName;
    }

    public CorporateOwner TransformToCorporateOwner(TaxId taxId) => new CorporateOwner(Id, Name, taxId, State, CreatedAt);
}