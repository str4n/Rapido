using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rapido.Services.Customers.Core.Entities.Lockout;

namespace Rapido.Services.Customers.Core.EF.Configs;

internal sealed class LockoutConfiguration : IEntityTypeConfiguration<Lockout>
{
    public void Configure(EntityTypeBuilder<Lockout> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Reason)
            .IsRequired();
        
        builder.Property(x => x.Description)
            .IsRequired();
        
        builder.HasDiscriminator<string>("Type")
            .HasValue<TemporaryLockout>(nameof(TemporaryLockout))
            .HasValue<PermanentLockout>(nameof(PermanentLockout));
    }
}