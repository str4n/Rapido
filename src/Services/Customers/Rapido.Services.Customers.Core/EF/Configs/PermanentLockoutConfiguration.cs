using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rapido.Services.Customers.Core.Entities.Lockout;

namespace Rapido.Services.Customers.Core.EF.Configs;

internal sealed class PermanentLockoutConfiguration : IEntityTypeConfiguration<PermanentLockout>
{
    public void Configure(EntityTypeBuilder<PermanentLockout> builder)
    {
    }
}