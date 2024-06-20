using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rapido.Services.Users.Core.Entities.Role;
using Rapido.Services.Users.Core.Entities.User;

namespace Rapido.Services.Users.Core.EF.Configs;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Email).IsUnique();
        
        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(300)
            .HasConversion(x => x.Value, x => new(x));

        builder.Property(x => x.State).HasConversion<string>();
        
        builder.Property(x => x.Password).IsRequired().HasMaxLength(300);
    }
}