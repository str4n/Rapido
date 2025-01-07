using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Rapido.Services.Users.Core.Shared.EF.Configs;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User.Domain.User>
{
    public void Configure(EntityTypeBuilder<User.Domain.User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Email).IsUnique();
        
        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(300)
            .HasConversion(x => x.Value, x => new(x));
        
        builder.Property(x => x.Type).HasConversion<string>();
        
        builder.Property(x => x.Password).IsRequired().HasMaxLength(300);
    }
}