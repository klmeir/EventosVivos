using EventosVivos.Application.Interfaces;
using EventosVivos.Domain.Ports;
using MediatR;

namespace EventosVivos.Application.Features.Reservations.Commands
{
    public record CancelReservationCommand(Guid ReservationId) : IRequest<bool>;

    public class CancelReservationCommandHandler(
        IReservationRepository reservationRepository,
        IEventRepository eventRepository,
        IUnitOfWork unitOfWork)
        : IRequestHandler<CancelReservationCommand, bool>
    {
        public async Task<bool> Handle(CancelReservationCommand request, CancellationToken cancellationToken)
        {
            var reservation = await reservationRepository.GetByIdAsync(request.ReservationId)
                ?? throw new KeyNotFoundException("The reservation could not be found.");

            var eventObj = await eventRepository.GetByIdAsync(reservation.EventId)
                ?? throw new KeyNotFoundException("No associated event found.");
            
            bool ticketsReleased = reservation.Cancel(eventObj.Schedule.StartTime, DateTime.UtcNow);

            if (ticketsReleased)
            {
                eventObj.ReleaseTickets(reservation.Quantity);
                await eventRepository.UpdateAsync(eventObj);
            }

            await reservationRepository.UpdateAsync(reservation);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
