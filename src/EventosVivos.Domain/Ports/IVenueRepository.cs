namespace EventosVivos.Domain.Ports
{
    public interface IVenueRepository
    {
        // Solo necesitamos la capacidad para validar RN-01 en el momento de crear el Evento
        Task<int?> GetVenueCapacityAsync(Guid venueId);
    }
}
