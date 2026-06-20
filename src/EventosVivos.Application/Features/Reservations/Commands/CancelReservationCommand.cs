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
                ?? throw new KeyNotFoundException("Reserva no encontrada.");

            var eventObj = await eventRepository.GetByIdAsync(reservation.EventId)
                ?? throw new KeyNotFoundException("Evento asociado no encontrado.");

            // Ejecutamos cancelación validando RN-07 y aforos
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
