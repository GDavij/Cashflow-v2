using Cashflow.Core;
using Cashflow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cashflow.Infrastructure.DataAccess.Mappings;

internal class CategoryMapping : OwnableEntityMapping<Category, long, long>
{
    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        base.Configure(builder);

        builder.Property(c => c.MaximumBudgetInvestment)
               .HasColumnName("MaximumBudgetInvestment");

        builder.Property(c => c.MaximumMoneyInvestment)
               .HasColumnName("MaximumMoneyInvestment")
               .HasColumnType("NUMERIC(12, 4)");

        builder.Property(c => c.Name)
               .HasColumnName("Name")
               .IsRequired();
    }
}