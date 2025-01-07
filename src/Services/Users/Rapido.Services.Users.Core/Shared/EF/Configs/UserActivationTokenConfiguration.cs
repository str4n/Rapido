using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rapido.Services.Users.Core.UserActivation.Domain;

namespace Rapido.Services.Users.Core.Shared.EF.Configs;

internal sealed class UserActivationTokenConfiguration : IEntityTypeConfiguration<UserActivationToken>
{
    public void Configure(EntityTypeBuilder<UserActivationToken> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Token).IsUnique();

        builder.Property(x => x.Token).IsRequired();

        builder.Property(x => x.UserId).IsRequired();

        builder.HasOne<User.Domain.User>().WithMany().HasForeignKey(x => x.UserId);
    }
}