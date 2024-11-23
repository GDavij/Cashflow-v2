using Cashflow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cashflow.Infrastructure.DataAccess.Mappings;

internal class AuditionEventMapping : ValueObjectMapping<AuditionEvent, long>
{
    public override void Configure(EntityTypeBuilder<AuditionEvent> builder)
    {
        base.Configure(builder);

        builder.Property(a => a.Event)
               .HasColumnName("Event")
               .IsRequired();

        builder.Property(a => a.IpAddress)
               .HasColumnName("IpAddress");

        builder.Property(a => a.OccuredAt)
               .HasColumnName("OccuredAt")
               .IsRequired();

        builder.Property(a => a.PrivateEvent)
               .HasColumnName("PrivateEvent")
               .IsRequired();

        builder.Property(a => a.TraceIdentifier)
               .HasColumnName("TraceId")
               .IsRequired();

        builder.Property(a => a.UserAgent)
               .HasColumnName("UserAgent");

        builder.HasOne(a => a.User)
               .WithMany()
               .HasForeignKey(a => a.UserId);
    }
}