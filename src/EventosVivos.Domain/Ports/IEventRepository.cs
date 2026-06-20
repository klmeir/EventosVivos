using EventosVivos.Domain.Entities;
using EventosVivos.Domain.Enums;

namespace EventosVivos.Domain.Ports
{
    public interface IEventRepository
    {
        // Operaciones CRUD básicas
        Task<Event?> GetByIdAsync(Guid id);
        Task AddAsync(Event newEvent);
        Task UpdateAsync(Event updatedEvent);

        // Método necesario para la RN-02 en el Domain Service
        Task<bool> HasOverlappingEventsAsync(Guid venueId, DateTime startTime, DateTime endTime);

        // Método necesario para el RF-02 en la capa de Aplicación (Listado con filtros)
        Task<List<Event>> SearchAsync(EventType? type, DateTime? startDate, Guid? venueId, EventStatus? status, string? titleSearch);
    }
}
