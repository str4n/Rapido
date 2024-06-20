using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Wallets.Aggregate;
using Rapido.Services.Wallets.Domain.Wallets.Wallet;

namespace Rapido.Services.Wallets.Infrastructure.EF.Configs;

internal sealed class WalletConfiguration : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new(x));
        
        builder.Property(x => x.CreatedAt).IsRequired();
        
        builder.Property(x => x.Version).IsConcurrencyToken();
        
        builder.Ignore(x => x.Events);
        
        builder.HasOne<Owner>().WithMany().HasForeignKey(x => x.OwnerId);
        builder.HasMany(x => x.Transfers).WithOne().HasForeignKey(x => x.WalletId);
        builder.HasMany(x => x.Balances).WithOne().HasForeignKey(x => x.WalletId);
    }
}