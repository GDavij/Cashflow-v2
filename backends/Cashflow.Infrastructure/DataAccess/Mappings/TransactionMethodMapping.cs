using Cashflow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cashflow.Infrastructure.DataAccess.Mappings;

internal class TransactionMethodMapping : ValueObjectMapping<TransactionMethod, short>
{
    public override void Configure(EntityTypeBuilder<TransactionMethod> builder)
    {
        base.Configure(builder);

        builder.Property(t => t.Name)
               .HasColumnName("Name")
               .IsRequired();
    }
}