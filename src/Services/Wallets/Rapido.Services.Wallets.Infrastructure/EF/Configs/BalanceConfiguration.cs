using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rapido.Services.Wallets.Domain.Wallets.Balance;
using Rapido.Services.Wallets.Domain.Wallets.Wallet;

namespace Rapido.Services.Wallets.Infrastructure.EF.Configs;

internal sealed class BalanceConfiguration : IEntityTypeConfiguration<Balance>
{
    public void Configure(EntityTypeBuilder<Balance> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new(x));

        builder.Property(x => x.Amount)
            .HasConversion(x => x.Value, x => new(x));

        builder.Property(x => x.Currency)
            .HasConversion(x => x.Value, x => new(x));

        builder.Property(x => x.IsPrimary).HasConversion<string>();

        builder.HasOne<Wallet>().WithMany().HasForeignKey(x => x.WalletId);
    }
}