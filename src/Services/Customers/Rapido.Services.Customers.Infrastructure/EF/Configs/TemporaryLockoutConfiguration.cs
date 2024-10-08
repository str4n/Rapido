﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rapido.Services.Customers.Domain.Common.Lockout;

namespace Rapido.Services.Customers.Infrastructure.EF.Configs;

internal sealed class TemporaryLockoutConfiguration : IEntityTypeConfiguration<TemporaryLockout>
{
    public void Configure(EntityTypeBuilder<TemporaryLockout> builder)
    {
    }
}