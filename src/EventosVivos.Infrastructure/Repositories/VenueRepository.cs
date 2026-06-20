using EventosVivos.Domain.Ports;

namespace EventosVivos.Infrastructure.Repositories
{
    public class VenueRepository : IVenueRepository
    {
        // Diccionario en memoria con los datos de referencia proporcionados en la prueba.
        // Usamos Guids constantes para poder referenciarlos fácilmente desde la UI/API.
        private static readonly Dictionary<Guid, int> _venues = new()
        {
            // 1 | Auditorio Central | 200 | Bogotá
            { Guid.Parse("11111111-1111-1111-1111-111111111111"), 200 }, 
        
            // 2 | Sala Norte | 50 | Bogotá
            { Guid.Parse("22222222-2222-2222-2222-222222222222"), 50 },  
        
            // 3 | Arena Sur | 500 | Medellín
            { Guid.Parse("33333333-3333-3333-3333-333333333333"), 500 }
        };

        public Task<int?> GetVenueCapacityAsync(Guid venueId)
        {
            if (_venues.TryGetValue(venueId, out var capacity))
            {
                return Task.FromResult<int?>(capacity);
            }

            return Task.FromResult<int?>(null); // Retorna null si el venue no existe
        }
    }
}
