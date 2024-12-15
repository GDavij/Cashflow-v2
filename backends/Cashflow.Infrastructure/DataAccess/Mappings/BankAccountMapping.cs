using Cashflow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cashflow.Infrastructure.DataAccess.Mappings;

internal class BankAccountMapping : OwnableEntityMapping<BankAccount>
{
    public override void Configure(EntityTypeBuilder<BankAccount> builder)
    {
        base.Configure(builder);

        builder.HasOne(b => b.AccountType)
               .WithMany(b => b.BankAccounts)
               .HasForeignKey(b => b.AccountTypeId);

        builder.Property(b => b.CurrentValue)
            .HasColumnName("CurrentValue")
            .HasColumnType("NUMERIC(12, 4)")
            .IsRequired();

        builder.Property(b => b.Name)
            .HasColumnName("Name")
            .IsRequired();
    }
}