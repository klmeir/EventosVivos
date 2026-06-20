using EventosVivos.Application.DTOs;
using EventosVivos.Domain.Ports;
using MediatR;

namespace EventosVivos.Application.Features.Events.Queries
{
    public record GetEventReportQuery(Guid EventId) : IRequest<EventReportDto>;

    public class GetEventReportQueryHandler(
        IEventRepository eventRepository,
        IReservationRepository reservationRepository)
        : IRequestHandler<GetEventReportQuery, EventReportDto>
    {
        public async Task<EventReportDto> Handle(GetEventReportQuery request, CancellationToken cancellationToken)
        {
            var eventObj = await eventRepository.GetByIdAsync(request.EventId)
                ?? throw new KeyNotFoundException("Evento no encontrado.");

            var reservations = await reservationRepository.GetConfirmedByEventIdAsync(request.EventId);

            int totalSold = reservations.Sum(r => r.Quantity);
            decimal occupancy = eventObj.MaxCapacity == 0 ? 0 : ((decimal)totalSold / eventObj.MaxCapacity) * 100;
            decimal totalRevenue = totalSold * eventObj.Price.Amount;

            return new EventReportDto(
                TotalTicketsSold: totalSold,
                TotalTicketsAvailable: eventObj.AvailableTickets,
                OccupancyPercentage: Math.Round(occupancy, 2),
                TotalRevenue: totalRevenue,
                Status: eventObj.CurrentStatus.ToString()
            );
        }
    }
}
