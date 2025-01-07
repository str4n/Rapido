using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rapido.Services.Users.Core.User.Domain;

namespace Rapido.Services.Users.Core.Shared.EF.Configs;

internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(x => x.Name);
        
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
    }
}