using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rapido.Services.Customers.Domain.Individual.Customer;

namespace Rapido.Services.Customers.Infrastructure.EF.Configs;

internal sealed class IndividualCustomerConfiguration : IEntityTypeConfiguration<IndividualCustomer>
{
    public void Configure(EntityTypeBuilder<IndividualCustomer> builder)
    {
        builder.Property(x => x.FullName)
            .HasConversion(x => x.Value, x => new(x));
    }
}