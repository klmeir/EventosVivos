using EventosVivos.Domain.Entities;

namespace EventosVivos.Domain.Ports
{
    public interface IVenueRepository
    {
        Task<IEnumerable<Venue>> GetAllAsync();
        Task<int?> GetVenueCapacityAsync(Guid venueId);
    }
}
