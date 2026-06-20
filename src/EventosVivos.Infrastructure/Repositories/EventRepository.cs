using EventosVivos.Domain.Entities;
using EventosVivos.Domain.Enums;
using EventosVivos.Domain.Ports;
using EventosVivos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EventosVivos.Infrastructure.Repositories
{
    public class EventRepository : GenericRepository<Event>, IEventRepository
    {
        public EventRepository(EventosVivosDbContext dbContext) : base(dbContext) { }

        // Implementación para la RN-02
        public async Task<bool> HasOverlappingEventsAsync(Guid venueId, DateTime startTime, DateTime endTime)
        {
            return await DbSet.AnyAsync(e =>
                e.VenueId == venueId &&
                EF.Property<EventStatus>(e, "_status") == EventStatus.Active &&
                e.Schedule.StartTime < endTime &&
                e.Schedule.EndTime > startTime);
        }

        // Implementación para RF-02 (Búsqueda)
        public async Task<List<Event>> SearchAsync(EventType? type, DateTime? startDate, Guid? venueId, EventStatus? status, string? titleSearch)
        {
            var query = DbSet.AsQueryable();

            if (type.HasValue)
                query = query.Where(e => e.Type == type.Value);

            if (startDate.HasValue)
                query = query.Where(e => e.Schedule.StartTime >= startDate.Value);

            if (venueId.HasValue)
                query = query.Where(e => e.VenueId == venueId.Value);

            if (status.HasValue)
                query = query.Where(e => EF.Property<EventStatus>(e, "_status") == status.Value);

            if (!string.IsNullOrWhiteSpace(titleSearch))
                query = query.Where(e => e.Title.ToLower().Contains(titleSearch.ToLower()));

            return await query.ToListAsync();
        }
    }
}
