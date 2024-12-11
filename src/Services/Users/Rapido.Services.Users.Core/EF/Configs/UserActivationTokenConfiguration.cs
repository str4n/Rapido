using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rapido.Services.Users.Core.Entities.ActivationToken;
using Rapido.Services.Users.Core.Entities.User;

namespace Rapido.Services.Users.Core.EF.Configs;

internal sealed class UserActivationTokenConfiguration : IEntityTypeConfiguration<UserActivationToken>
{
    public void Configure(EntityTypeBuilder<UserActivationToken> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Token).IsUnique();

        builder.Property(x => x.Token).IsRequired();

        builder.Property(x => x.UserId).IsRequired();

        builder.HasOne<User>().WithMany().HasForeignKey(x => x.UserId);
    }
}