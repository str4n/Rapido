using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rapido.Services.Wallets.Domain.Owners.Owner;

namespace Rapido.Services.Wallets.Infrastructure.EF.Configs;

internal sealed class IndividualOwnerConfiguration : IEntityTypeConfiguration<IndividualOwner>
{
    public void Configure(EntityTypeBuilder<IndividualOwner> builder)
    {
        builder.Property(x => x.FullName)
            .HasConversion(x => x.Value, x => new(x))
            .IsRequired();
    }
}