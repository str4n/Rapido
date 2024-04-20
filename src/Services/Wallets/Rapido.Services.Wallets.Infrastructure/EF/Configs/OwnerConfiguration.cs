using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rapido.Services.Wallets.Domain.Owners.Owner;

namespace Rapido.Services.Wallets.Infrastructure.EF.Configs;

internal sealed class OwnerConfiguration : IEntityTypeConfiguration<Owner>
{
    public void Configure(EntityTypeBuilder<Owner> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new(x));

        builder.Property(x => x.Name)
            .HasConversion(x => x.Value, x => new(x))
            .IsRequired();

        builder.Property(x => x.State)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();
        
        builder.HasDiscriminator<string>("Type")
            .HasValue<IndividualOwner>(nameof(IndividualOwner))
            .HasValue<CorporateOwner>(nameof(CorporateOwner));
    }
}