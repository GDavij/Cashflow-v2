using Cashflow.Core;
using Cashflow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cashflow.Infrastructure.DataAccess.Mappings;

internal class TransactionMapping : OwnableEntityMapping<Transaction> 
{
    public override void Configure(EntityTypeBuilder<Transaction> builder)
    {
        base.Configure(builder);

        builder.HasOne(t => t.BankAccount)
            .WithMany(b => b.Transactions)
            .HasForeignKey(t => t.BankAccountId)
            .IsRequired();

        builder.HasOne(t => t.Category)
               .WithMany(c => c.Transactions)
               .HasForeignKey(t => t.CategoryId)
               .IsRequired();

        builder.Property(t => t.Description)
               .HasColumnName("Description")
               .IsRequired();

        builder.Property(t => t.DoneAt)
               .HasColumnName("DoneAt")
               .IsRequired();

        builder.Property(t => t.Month)
               .HasColumnName("Month")
               .IsRequired();

        builder.Property(t => t.Year)
               .HasColumnName("Year")
               .IsRequired();

        builder.Property(t => t.Value)
               .HasColumnName("Value")
               .IsRequired();

        builder.HasOne(t => t.TransactionMethod)
               .WithMany()
               .HasForeignKey(t => t.TransactionMethodId)
               .IsRequired();
    }
}