using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rapido.Services.Users.Core.PasswordRecovery.Domain;

namespace Rapido.Services.Users.Core.Shared.EF.Configs;

internal sealed class PasswordRecoveryTokenConfiguration : IEntityTypeConfiguration<PasswordRecoveryToken>
{
    public void Configure(EntityTypeBuilder<PasswordRecoveryToken> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Token).IsUnique();

        builder.Property(x => x.Token).IsRequired();

        builder.Property(x => x.UserId).IsRequired();

        builder.HasOne<User.Domain.User>().WithMany().HasForeignKey(x => x.UserId);
    }
}