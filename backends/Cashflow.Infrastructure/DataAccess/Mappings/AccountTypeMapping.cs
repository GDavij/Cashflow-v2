using Cashflow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cashflow.Infrastructure.DataAccess.Mappings;

internal class AccountTypeMapping : ValueObjectMapping<AccountType, short>
{
    public override void Configure(EntityTypeBuilder<AccountType> builder)
    {
        base.Configure(builder);

        builder.Property(a => a.Name)
               .HasColumnName("Name")
               .IsRequired();
    }
}