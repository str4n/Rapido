using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rapido.Services.Customers.Core.Entities.Customer;

namespace Rapido.Services.Customers.Core.EF.Configs;

internal sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Email).IsUnique();

        builder.Property(x => x.Email)
            .HasConversion(x => x.Value, x => new(x))
            .IsRequired();

        builder.Property(x => x.Name)
            .HasConversion(x => x.Value, x => new(x));

        builder.Property(x => x.FullName)
            .HasConversion(x => x.Value, x => new(x));

        builder.Property(x => x.Nationality)
            .HasConversion(x => x.Value, x => new(x));

        builder.Property(x => x.State)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.Address)
            .HasColumnType("jsonb");
        
        builder.Property(x => x.Identity)
            .HasColumnType("jsonb");
    }
}