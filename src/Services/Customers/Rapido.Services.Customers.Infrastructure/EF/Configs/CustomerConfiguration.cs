﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using Rapido.Services.Customers.Domain.Common.Customer;
using Rapido.Services.Customers.Domain.Corporate.Customer;
using Rapido.Services.Customers.Domain.Individual.Customer;

namespace Rapido.Services.Customers.Infrastructure.EF.Configs;

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

        builder.Property(x => x.Nationality)
            .HasConversion(x => x.Value, x => new(x));
        

        builder.Property(x => x.IsLocked)
            .HasConversion<string>();
        
        builder.Property(x => x.IsCompleted)
            .HasConversion<string>();

        builder.HasDiscriminator<string>("Type")
            .HasValue<IndividualCustomer>("Individual")
            .HasValue<CorporateCustomer>("Corporate");

        builder.Property(x => x.Address)
            .HasColumnType("text")
            .HasConversion(
                x => JsonConvert.SerializeObject(x), 
                x => JsonConvert.DeserializeObject<Address>(x));
    }
}