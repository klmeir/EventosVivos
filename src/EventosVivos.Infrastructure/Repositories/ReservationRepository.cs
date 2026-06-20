using Microsoft.EntityFrameworkCore;
using EventosVivos.Domain.Entities;
using EventosVivos.Domain.Ports;
using EventosVivos.Domain.Enums;
using EventosVivos.Infrastructure.Data;

namespace EventosVivos.Infrastructure.Repositories
{
    public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository
    {
        public ReservationRepository(EventosVivosDbContext dbContext) : base(dbContext) { }

        public async Task<List<Reservation>> GetByEventIdAsync(Guid eventId)
        {
            return await DbSet
                .Where(r => r.EventId == eventId)
                .AsNoTracking().ToListAsync();
        }

        // Implementación para RF-06
        public async Task<List<Reservation>> GetConfirmedByEventIdAsync(Guid eventId)
        {
            return await DbSet
                .Where(r => r.EventId == eventId && r.Status == ReservationStatus.Confirmed)
                .AsNoTracking().ToListAsync();
        }
    }
}
