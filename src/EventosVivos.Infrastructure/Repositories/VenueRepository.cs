using EventosVivos.Domain.Entities;
using EventosVivos.Domain.Ports;
using EventosVivos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EventosVivos.Infrastructure.Repositories
{
    public class VenueRepository : GenericRepository<Venue>, IVenueRepository
    {
        public VenueRepository(EventosVivosDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<Venue>> GetAllAsync()
        {
            return await DbSet                
                .AsNoTracking().ToListAsync();
        }

        public async Task<int?> GetVenueCapacityAsync(Guid venueId)
        {
            var venue = await GetByIdAsync(venueId);

            return venue?.Capacity;
        }
    }
}
