using Cashflow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cashflow.Infrastructure.DataAccess.Mappings;

internal class RecurrencyTimeMapping : ValueObjectMapping<Domain.Entities.RecurrencyTime, short>
{
    public override void Configure(EntityTypeBuilder<Domain.Entities.RecurrencyTime> builder)
    {
        base.Configure(builder);

        builder.Property(r => r.Name)
               .HasColumnName("Name")
               .IsRequired();
    }
}