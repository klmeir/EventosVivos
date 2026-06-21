using EventosVivos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventosVivos.Infrastructure.Data.Configurations
{
    public class VenueConfiguration : IEntityTypeConfiguration<Venue>
    {
        public void Configure(EntityTypeBuilder<Venue> builder)
        {            
            builder.HasKey(v => v.Id);
            
            builder.Property(v => v.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(v => v.City)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(v => v.Capacity)
                .IsRequired();
            
            builder.HasData(
                new Venue(
                    Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    "Auditorio Central", 200, "Bogotá"),
                new Venue(
                    Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    "Sala Norte", 50, "Bogotá"),
                new Venue(
                    Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    "Arena Sur", 500, "Medellín")
            );
        }
    }
}
