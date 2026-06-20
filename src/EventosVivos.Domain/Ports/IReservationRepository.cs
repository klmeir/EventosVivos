using EventosVivos.Domain.Entities;

namespace EventosVivos.Domain.Ports
{
    public interface IReservationRepository
    {
        // Operaciones CRUD básicas
        Task<Reservation?> GetByIdAsync(Guid id);
        Task AddAsync(Reservation newReservation);
        Task UpdateAsync(Reservation updatedReservation);
        Task<List<Reservation>> GetByEventIdAsync(Guid eventId);

        // Método necesario para el RF-06 en la capa de Aplicación (Reporte de Ocupación)
        Task<List<Reservation>> GetConfirmedByEventIdAsync(Guid eventId);
    }
}
