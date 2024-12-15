using Cashflow.Core;
using Cashflow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cashflow.Infrastructure.DataAccess.Mappings;

internal class RecurrencyMapping : OwnableEntityMapping<Recurrency>
{
    public override void Configure(EntityTypeBuilder<Recurrency> builder)
    {
        base.Configure(builder);

        builder.Property(r => r.Times)
               .HasColumnName("Times")
               .IsRequired();

        builder.HasOne(r => r.RecurrencyTime)
                .WithMany()
                .HasForeignKey(r => r.RecurrencyTimeId)
                .IsRequired();

        builder.HasOne(r => r.TransactionMethod)
               .WithMany()
               .HasForeignKey(r => r.RecurrencyTimeId)
               .IsRequired();

        builder.Property(r => r.TransactionValue)
               .HasColumnName("TransactionValue")
               .IsRequired();
    }
}