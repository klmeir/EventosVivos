using EventosVivos.Application.DTOs;
using EventosVivos.Domain.Ports;
using MediatR;

namespace EventosVivos.Application.Features.Venues.Queries
{    
    public record GetVenuesQuery() : IRequest<List<VenueDto>>;

    public class GetVenuesQueryHandler(IVenueRepository venueRepository)
        : IRequestHandler<GetVenuesQuery, List<VenueDto>>
    {
        public async Task<List<VenueDto>> Handle(GetVenuesQuery request, CancellationToken cancellationToken)
        {            
            var venues = await venueRepository.GetAllAsync();
            return venues.Select(v => new VenueDto(v.Id, v.Name, v.Capacity, v.City)).ToList();
        }
    }
}
