using Cashflow.Core;
using Cashflow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cashflow.Infrastructure.DataAccess.Mappings;

internal class UserMapping : OwnableEntityMapping<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.Property(u => u.BirthDate)
               .HasColumnName("BirthDate")
               .IsRequired();

        builder.Property(u => u.Email)
               .HasColumnName("Email")
               .IsRequired();

        builder.Property(u => u.Passphrase)
               .HasColumnName("Passphrase")
               .IsRequired();

        builder.HasOne(u => u.Role)
            .WithMany()
            .HasForeignKey(u => u.RoleId);

        builder.Property(u => u.Username)
               .HasColumnName("Username")
               .IsRequired();
    }
}