using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rapido.Services.Customers.Domain.Corporate.Customer;

namespace Rapido.Services.Customers.Infrastructure.EF.Configs;

internal sealed class CorporateCustomerConfiguration : IEntityTypeConfiguration<CorporateCustomer>
{
    public void Configure(EntityTypeBuilder<CorporateCustomer> builder)
    {
        builder.Property(x => x.TaxId)
            .HasConversion(x => x.Value, x => new(x));
    }
}