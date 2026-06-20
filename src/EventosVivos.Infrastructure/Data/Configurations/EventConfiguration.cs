using EventosVivos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventosVivos.Infrastructure.Data.Configurations
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Title).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Description).IsRequired().HasMaxLength(500);

            builder.Property(e => e.AvailableTickets)
                .HasColumnName("AvailableTickets")
                .IsRequired();

            // Mapeo de Value Objects (OwnsOne)
            builder.OwnsOne(e => e.Schedule, s =>
            {
                s.Property(t => t.StartTime).HasColumnName("StartTime").IsRequired();
                s.Property(t => t.EndTime).HasColumnName("EndTime").IsRequired();
            });

            builder.OwnsOne(e => e.Price, p =>
            {
                p.Property(m => m.Amount).HasColumnName("Price").HasColumnType("decimal(18,2)");
            });

            // Persistiendo el campo privado _status
            builder.Property("_status").HasColumnName("Status").IsRequired();

            // Ignoramos la propiedad calculada
            builder.Ignore(e => e.CurrentStatus);
        }
    }
}
