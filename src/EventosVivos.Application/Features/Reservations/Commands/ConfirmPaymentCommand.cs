using EventosVivos.Application.Interfaces;
using EventosVivos.Domain.Ports;
using MediatR;

namespace EventosVivos.Application.Features.Reservations.Commands
{
    public record ConfirmPaymentCommand(Guid ReservationId) : IRequest<string>;

    public class ConfirmPaymentCommandHandler(IReservationRepository reservationRepository, IUnitOfWork unitOfWork)
        : IRequestHandler<ConfirmPaymentCommand, string>
    {
        public async Task<string> Handle(ConfirmPaymentCommand request, CancellationToken cancellationToken)
        {
            var reservation = await reservationRepository.GetByIdAsync(request.ReservationId)
                ?? throw new KeyNotFoundException("The reservation could not be found.");

            reservation.ConfirmPayment();
            await reservationRepository.UpdateAsync(reservation);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return reservation.Code;
        }
    }
}
