using Cashflow.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cashflow.Infrastructure.DataAccess.Mappings;

internal abstract class OwnableEntityMapping<T> : IEntityTypeConfiguration<T>
    where T : OwnableEntity<T>
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("Id");

        builder.Property(e => e.Active).HasColumnName("Active").IsRequired();

        builder.Property(e => e.Deleted).HasColumnName("Deleted").IsRequired();

        builder.Property(e => e.LastModifiedBy).HasColumnName("LastModifiedBy");
        
        builder.Property(e => e.LastModifiedAt).HasColumnName("LastModifiedAt");

        builder.Property(e => e.OwnerId).HasColumnName("OwnerId");

        builder.Property(e => e.CreatedAt).HasColumnName("CreatedAt").IsRequired();
    }
}