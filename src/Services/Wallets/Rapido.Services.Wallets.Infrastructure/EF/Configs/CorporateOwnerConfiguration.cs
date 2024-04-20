using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rapido.Services.Wallets.Domain.Owners.Owner;

namespace Rapido.Services.Wallets.Infrastructure.EF.Configs;

internal sealed class CorporateOwnerConfiguration : IEntityTypeConfiguration<CorporateOwner>
{
    public void Configure(EntityTypeBuilder<CorporateOwner> builder)
    {
        builder.Property(x => x.TaxId)
            .HasConversion(x => x.Value, x => new(x))
            .IsRequired();
    }
}