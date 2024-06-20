using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Wallets.Aggregate;
using Rapido.Services.Wallets.Domain.Wallets.Transfer;
using Rapido.Services.Wallets.Domain.Wallets.Wallet;

namespace Rapido.Services.Wallets.Infrastructure.EF.Configs;

internal sealed class TransferConfiguration : IEntityTypeConfiguration<Transfer>
{
    public void Configure(EntityTypeBuilder<Transfer> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new(x));

        builder.Property(x => x.WalletId).HasConversion(x => x.Value, x => new(x));

        builder.Property(x => x.Name)
            .HasConversion(x => x.Value, x => new(x))
            .IsRequired();

        builder.Property(x => x.Currency)
            .HasConversion(x => x.Value, x => new(x))
            .IsRequired();

        builder.Property(x => x.Amount)
            .HasConversion(x => x.Value, x => new(x))
            .IsRequired();
        
        builder.Property(x => x.Metadata)
            .HasConversion(x => x.Value, x => new(x))
            .IsRequired();

        builder.Property(x => x.CreatedAt).IsRequired();

        builder.HasDiscriminator<string>("Type")
            .HasValue<IncomingTransfer>(nameof(IncomingTransfer))
            .HasValue<OutgoingTransfer>(nameof(OutgoingTransfer));
    }
}