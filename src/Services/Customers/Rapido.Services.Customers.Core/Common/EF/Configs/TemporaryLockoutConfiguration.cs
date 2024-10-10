using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rapido.Services.Customers.Core.Common.Domain.Lockout;

namespace Rapido.Services.Customers.Core.Common.EF.Configs;

internal sealed class TemporaryLockoutConfiguration : IEntityTypeConfiguration<TemporaryLockout>
{
    public void Configure(EntityTypeBuilder<TemporaryLockout> builder)
    {
    }
}