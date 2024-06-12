using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
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

        builder.Property(x => x.Type)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.StateBeforeLockout)
            .HasConversion<string>();

        builder.Property(x => x.Address)
            .HasColumnType("text")
            .HasConversion(
                x => JsonConvert.SerializeObject(x), 
                x => JsonConvert.DeserializeObject<Address>(x));

        builder.Property(x => x.Identity)
            .HasColumnType("text")
            .HasConversion(
                x => ConvertIdentity(x), 
                x => JsonConvert.DeserializeObject<Identity>(x));
    }

    private string ConvertIdentity(Identity identity)
    {
        var type = identity.Type.ToString();
        var series = identity.Series;
        
        return JsonConvert.SerializeObject(new {type, series});
    }
}