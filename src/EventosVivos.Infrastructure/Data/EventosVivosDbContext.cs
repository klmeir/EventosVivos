using EventosVivos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventosVivos.Infrastructure.Data
{
    public class EventosVivosDbContext : DbContext
    {
        public DbSet<Event> Events => Set<Event>();
        public DbSet<Reservation> Reservations => Set<Reservation>();

        public EventosVivosDbContext(DbContextOptions<EventosVivosDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Esto busca todas las clases IEntityTypeConfiguration en este ensamblado y las aplica
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventosVivosDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
