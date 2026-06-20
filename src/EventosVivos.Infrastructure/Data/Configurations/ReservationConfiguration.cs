using EventosVivos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventosVivos.Infrastructure.Data.Configurations
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.BuyerName).IsRequired().HasMaxLength(150);

            // Mapeo del Email (Value Object)
            builder.OwnsOne(r => r.BuyerEmail, e =>
            {
                e.Property(m => m.Value).HasColumnName("BuyerEmail").IsRequired();
            });
        }
    }
}
