using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Transfer;

namespace Rapido.Services.Wallets.Infrastructure.EF.Configs;

internal sealed class InternalTransferConfiguration : IEntityTypeConfiguration<InternalTransfer>
{
    public void Configure(EntityTypeBuilder<InternalTransfer> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new(x));

        builder.Property(x => x.TransactionId)
            .HasConversion(x => x.Value, x => new(x));

        builder.Property(x => x.BalanceId)
            .HasConversion(x => x.Value, x => new(x));

        builder.Property(x => x.Currency)
            .HasConversion(x => x.Value, x => new(x))
            .IsRequired();

        builder.Property(x => x.Amount)
            .HasConversion(x => x.Value, x => new(x))
            .IsRequired();

        builder.Property(x => x.Metadata)
            .HasConversion(x => x.Value, x => new(x));

        builder.Property(x => x.CreatedAt).IsRequired();
        
        builder.Property(x => x.ExchangeRate)
            .HasColumnType("text")
            .HasConversion(
                x => JsonConvert.SerializeObject(x), 
                x => JsonConvert.DeserializeObject<ExchangeRate>(x));

        builder.HasDiscriminator<string>("Type")
            .HasValue<IncomingInternalTransfer>(nameof(IncomingInternalTransfer))
            .HasValue<OutgoingInternalTransfer>(nameof(OutgoingInternalTransfer));
    }
}