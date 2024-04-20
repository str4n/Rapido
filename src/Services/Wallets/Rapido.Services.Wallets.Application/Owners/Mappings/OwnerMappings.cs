using Rapido.Services.Wallets.Application.Owners.DTO;
using Rapido.Services.Wallets.Domain.Owners.Owner;

namespace Rapido.Services.Wallets.Application.Owners.Mappings;

internal static class OwnerMappings
{
    public static IndividualOwnerDto AsDto(this IndividualOwner owner) 
        => new(owner.Id, owner.Name, owner.FullName, owner.State.ToString(), owner.CreatedAt, owner.VerifiedAt);

    public static CorporateOwnerDto AsDto(this CorporateOwner owner)
        => new(owner.Id, owner.Name, owner.TaxId, owner.State.ToString(), owner.CreatedAt, owner.VerifiedAt);
}