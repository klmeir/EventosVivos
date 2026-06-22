using AutoMapper;
using EventosVivos.Application.DTOs;
using EventosVivos.Domain.Ports;
using MediatR;

namespace EventosVivos.Application.Features.Reservations.Queries
{
    public record GetReservationsByEventQuery(Guid EventId) : IRequest<List<ReservationDto>>;

    public class GetReservationsByEventQueryHandler(IReservationRepository reservationRepository, IMapper mapper)
        : IRequestHandler<GetReservationsByEventQuery, List<ReservationDto>>
    {
        public async Task<List<ReservationDto>> Handle(GetReservationsByEventQuery request, CancellationToken cancellationToken)
        {            
            var reservations = await reservationRepository.GetByEventIdAsync(request.EventId);

            return mapper.Map<List<ReservationDto>>(reservations);
        }
    }
}
