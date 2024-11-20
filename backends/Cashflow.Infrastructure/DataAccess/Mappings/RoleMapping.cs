using Cashflow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cashflow.Infrastructure.DataAccess.Mappings;

internal class RoleMapping : ValueObjectMapping<Role, short>
{
    public override void Configure(EntityTypeBuilder<Role> builder)
    {
        base.Configure(builder);

        builder.Property(r => r.Name)
               .HasColumnName("Role")
               .IsRequired();
    }
}