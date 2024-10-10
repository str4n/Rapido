using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rapido.Services.Customers.Core.Common.Domain.Lockout;

namespace Rapido.Services.Customers.Core.Common.EF.Configs;

internal sealed class PermanentLockoutConfiguration : IEntityTypeConfiguration<PermanentLockout>
{
    public void Configure(EntityTypeBuilder<PermanentLockout> builder)
    {
    }
}