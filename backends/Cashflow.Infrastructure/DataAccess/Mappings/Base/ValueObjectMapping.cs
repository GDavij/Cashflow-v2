using Cashflow.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cashflow.Infrastructure.DataAccess.Mappings;

internal abstract class ValueObjectMapping<T, TId> : IEntityTypeConfiguration<T>
    where TId : struct
    where T : ValueObject<TId>
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id).HasColumnName("Id");
    }
}